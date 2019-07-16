using PropellerMonkeys.Sockets;
using PropellerMonkeys.Sockets.Models;
using PropellerMonkeys.Sockets.ServerBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropellerMonkeys.WebApp.Sockets.Server.ConsoleClient
{
  public class ConsoleClientServer : ServerSocketBase
  {
    private Dictionary<string, ConsoleClient> _clients = new Dictionary<string, ConsoleClient>();
    protected ConsoleClient this[string uid] => _clients.ContainsKey(uid) ? _clients[uid] : null;
    public IEnumerable<ConsoleClientWrapper> CurrentClients { get => (from c in this._clients select c.Value.Wrapper).ToList(); }
    protected override bool CloseIfConnectionProblem(string uid) => this[uid].Activity == ConsoleClientActivity.FATAL;



    protected override Task OnClientConnect(string uid)
    {
      if (!_clients.ContainsKey(uid))
        _clients.Add(uid, new ConsoleClient(uid));

      WebSocketsMiddleware.FrontendServer.Log("[connection] client with uid " + uid);
      return Task.FromResult(0);
    }

    protected override Task OnClientDisconect(string uid)
    {
      if (_clients.ContainsKey(uid))
      {
        this[uid].OnBeforeDisconnect();
        WebSocketsMiddleware.FrontendServer.Log("[disconect] client with username = " + this[uid].Username);
        _clients.Remove(uid);
      }

      return Task.FromResult(0);
    }

    protected override Task OnReceiveMessage(string uid, ServerSocketResponse package)
    {
      try
      {
        SocketDistributionModel modelReceived = SocketDistributionManager.ConvertToModel(package.Stream.ToArray());
        switch(modelReceived.Type)
        {
          //
          // REGISTRATION right after socket is made

          case SocketDistributionModelType.LOGIN:
            var registrationModel = modelReceived.Convert<SocketRegistrationModel>();
            if (registrationModel == null)
            {
              WebSocketsMiddleware.FrontendServer.Log("[onreceive] FATAL = registration model could not be convered");
              return Task.FromResult(0);
            }
            this[uid].OnRegistration(registrationModel.Context);
            WebSocketsMiddleware.FrontendServer.Log("[connect] Connection registered with username=" + this[uid].Username);
            return Task.FromResult(0);

          //
          // INTERNAL for internal comunication between client/server

          case SocketDistributionModelType.INTERNAL:

            break;

          //
          // DEFAULT or main communication beetween client/server

          case SocketDistributionModelType.DEFAULT:
            var receivedModel = modelReceived.Convert<SocketModel>();
            switch(receivedModel.SubType)
            {
              case SocketModelTypes.PING:
                this[uid].OnPing(receivedModel as SocketPingModel);
                break;
            }

            break;
        }

      }
      catch (Exception e)
      {
        WebSocketsMiddleware.FrontendServer.Log("[onreceive] FATAL = " + e.ToString());
      }
      return Task.FromResult(0);
    }


    //
    // TESTS

    // Create dummy objects for testing
    public void CreateDummyConsoleClient(string uid)
    {
      ConsoleClient client = new ConsoleClient(uid);
      client.Context = new PropellerMonkeys.Sockets.Device.DeviceContextModel();
      client.Context.Username = Guid.NewGuid().ToString();
      client.Context.MachineName = Guid.NewGuid().ToString().Split('-')[0];
      _clients.Add(uid, client);
    }
  }
}
