using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PropellerMonkeys.Sockets.ServerBase
{
  public abstract class ServerSocketBase
  {
    private ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

    public WebSocket this[string id] => _sockets.ContainsKey(id) ? _sockets[id] : null;

    public async Task CallInvoke(HttpContext context)
    {
      CancellationToken ct = context.RequestAborted;
      WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
      var socketId = Guid.NewGuid().ToString();

      _sockets.TryAdd(socketId, currentSocket);
      await this.OnClientConnect(socketId);

      while (true)
      {
        if (ct.IsCancellationRequested)
          break;

        ServerSocketResponse package = await ReceiveResponseAsync(currentSocket);

        if (!package.HasTransimssion)
        {
          if (currentSocket.State != WebSocketState.Open)
            break;
          if (this.CloseIfConnectionProblem(socketId))
            break;
        }
        else
          await this.OnReceiveMessage(socketId, package);

      }

      WebSocket dummy;
      _sockets.TryRemove(socketId, out dummy);

      await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
      currentSocket.Dispose();
      await this.OnClientDisconect(socketId);
    }

    public string CreateDummySocket()
    {
      var socketId = Guid.NewGuid().ToString();
      _sockets.TryAdd(socketId, null);
      return socketId;
    }

    /*
    +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
      IMPLEMENTATIONS
    */

    private static async Task<ServerSocketResponse> ReceiveResponseAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
    {
      try
      {
        ServerSocketResponse response = new ServerSocketResponse();
        response.Stream = new MemoryStream();
        var buffer = new ArraySegment<byte>(new byte[8192]);

        WebSocketReceiveResult result;
        do
        {
          ct.ThrowIfCancellationRequested();

          result = await socket.ReceiveAsync(buffer, ct);
          response.Stream.Write(buffer.Array, buffer.Offset, result.Count);
        }
        while (!result.EndOfMessage);
        response.Stream.Seek(0, SeekOrigin.Begin);

        response.Type = result.MessageType;
        return response;
      }
      catch (Exception e)
      {
        return null;
      }
    }
    
    // Original
    private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
    {
      var buffer = Encoding.UTF8.GetBytes(data);
      var segment = new ArraySegment<byte>(buffer);
      return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
    }
    private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default(CancellationToken))
    {
      try
      {
        var buffer = new ArraySegment<byte>(new byte[8192]);
        using (var ms = new MemoryStream())
        {
          WebSocketReceiveResult result;
          do
          {
            ct.ThrowIfCancellationRequested();

            result = await socket.ReceiveAsync(buffer, ct);
            ms.Write(buffer.Array, buffer.Offset, result.Count);
          }
          while (!result.EndOfMessage);

          ms.Seek(0, SeekOrigin.Begin);
          if (result.MessageType != WebSocketMessageType.Text)
            return null;

          // Encoding UTF8: https://tools.ietf.org/html/rfc6455#section-5.6
          using (var reader = new StreamReader(ms, Encoding.UTF8))
            return await reader.ReadToEndAsync();
        }
      }
      catch(Exception e)
      {
        return string.Empty;
      }
    }

    // Sending data
    protected async Task SendToAll(SocketDistributionModel model)
    {
      byte[] data = model.DataType == SocketDistributionModelDataType.JSON ? model.Data : SocketDistributionManager.GetBytesFromObj(model);
      foreach (var socket in _sockets)
        if(socket.Value.State == WebSocketState.Open)
          await socket.Value.SendAsync(new ArraySegment<byte>(data), model.WebSocketMessageType, true, default(CancellationToken));
    }
    protected async Task Send(string uid, SocketDistributionModel model)
    {
      if (_sockets.ContainsKey(uid)) return;
      var socket = this[uid];

      byte[] data = model.DataType == SocketDistributionModelDataType.JSON ? model.Data : SocketDistributionManager.GetBytesFromObj(model);
      if (socket.State == WebSocketState.Open)
        await this[uid]?.SendAsync(data, model.WebSocketMessageType, true, default(CancellationToken));
    }


    /*
    +=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=
      APSTRACTIOn
    */

    protected virtual Task OnReceiveMessage(string uid, ServerSocketResponse package) => Task.FromResult(default(object));
    protected virtual Task OnClientConnect(string uid) => Task.FromResult(default(object));
    protected virtual Task OnClientDisconect(string uid) => Task.FromResult(default(object));
    protected virtual bool CloseIfConnectionProblem(string uid) => false;

  }
}
