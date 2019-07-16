using Microsoft.AspNetCore.Http;
using PropellerMonkeys.WebApp.Sockets.Server.ConsoleClient;
using PropellerMonkeys.WebApp.Sockets.Server.Frontend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PropellerMonkeys.WebApp.Sockets
{
  public class WebSocketsMiddleware
  {
    private readonly RequestDelegate _next;
    public static ConsoleClientServer ConsoleClientServer = new ConsoleClientServer();
    public static FrontendServer FrontendServer = new FrontendServer();


    public WebSocketsMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      if (!context.WebSockets.IsWebSocketRequest)
      {
        await _next.Invoke(context);
        return;
      }

      if (context.Request.Path.StartsWithSegments("/ws_frontend"))
        await FrontendServer.CallInvoke(context);

      if (context.Request.Path.StartsWithSegments("/ws_clientconsole"))
        await ConsoleClientServer.CallInvoke(context);

    }
  }
}
