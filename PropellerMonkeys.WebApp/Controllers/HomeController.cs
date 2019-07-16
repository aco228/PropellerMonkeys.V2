using Microsoft.AspNetCore.Mvc;
using PropellerMonkeys.Sockets;
using PropellerMonkeys.WebApp.Sockets;
using PropellerMonkeys.WebApp.Sockets.Server.Frontend.Models.Send;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropellerMonkeys.WebApp.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index() => this.Content("OK");

    public IActionResult Demo()
    {
      for(int i = 0; i < 15; i++)
      {
        string uid = WebSocketsMiddleware.ConsoleClientServer.CreateDummySocket();
        WebSocketsMiddleware.ConsoleClientServer.CreateDummyConsoleClient(uid);
      }
      return this.Content("OK");
    }

    public async Task<IActionResult> Test()
    {
      await WebSocketsMiddleware.FrontendServer.Send(new FrontendConsoleModel("test from controller"));
      return this.Content("OK");
    }
  }
}
