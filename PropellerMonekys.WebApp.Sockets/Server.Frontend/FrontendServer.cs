using PropellerMonkeys.Sockets;
using PropellerMonkeys.Sockets.ServerBase;
using PropellerMonkeys.WebApp.Sockets.Server.Frontend.Models;
using PropellerMonkeys.WebApp.Sockets.Server.Frontend.Models.Receive;
using PropellerMonkeys.WebApp.Sockets.Server.Frontend.Models.Send;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PropellerMonkeys.WebApp.Sockets.Server.Frontend
{
  public class FrontendServer : ServerSocketBase
  {

    public async Task Send(FrontendSendingWrapper wrapper) => await this.SendToAll(wrapper.Instantiate());

    public async void Log(string text)
    {
      Console.WriteLine(text);
      await this.Send(new FrontendConsoleModel(text));
    }

    protected override async Task OnReceiveMessage(string uid, ServerSocketResponse package)
    {
      if (package.Type != System.Net.WebSockets.WebSocketMessageType.Text)
        return;

      FrontendServerReceiveModelManager.ParseData(package);
    }

  }
}
