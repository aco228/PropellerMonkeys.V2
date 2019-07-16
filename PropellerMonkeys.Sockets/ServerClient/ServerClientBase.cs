using PropellerMonkeys.Sockets.Device;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace PropellerMonkeys.Sockets.ServerClient
{
  public abstract class ServerClientBase
  {
    protected WebSocket socket = null;

    public ServerClientBase(string url)
    {
      Console.WriteLine("Connecting to " + url);
      this.socket = new WebSocket(url: url, onMessage: OnMessage, onError: OnError);
      this.socket.Connect().Wait();
      Console.WriteLine("Connected to " + url);
    }

    public void Register(DeviceContextModel context)=> this.Send(new SocketRegistrationModel() { Context = context }.Instantiate());

    ~ServerClientBase()
    {
      this.socket.Close(CloseStatusCode.Normal);
      this.socket.Dispose();
    }

    public Task Send(SocketModel model) => this.Send(model.Instantiate());
    public Task Send(SocketDistributionModel model)
    {
      byte[] data = SocketDistributionManager.GetBytesFromObj(model);
      this.socket.Send(data);
      return Task.FromResult(0);
    }

    private Task OnError(WebSocketSharp.ErrorEventArgs errorEventArgs)
    {
      Console.Write("Error: {0}, Exception: {1}", errorEventArgs.Message, errorEventArgs.Exception);
      return Task.FromResult(0);
    }

    private Task OnMessage(MessageEventArgs messageEventArgs)
    {
      Console.Write("Message received: {0}", messageEventArgs.Text.ReadToEnd());
      return Task.FromResult(0);
    }

  }
}
