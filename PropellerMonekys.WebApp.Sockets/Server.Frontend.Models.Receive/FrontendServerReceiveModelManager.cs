using Newtonsoft.Json;
using PropellerMonkeys.Sockets.ServerBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.WebApp.Sockets.Server.Frontend.Models.Receive
{
  public static class FrontendServerReceiveModelManager
  {

    public static object ParseData(ServerSocketResponse input)
    {
      if (input.Type == System.Net.WebSockets.WebSocketMessageType.Binary)
        return null;

      try
      {
        FrontendModelWrapper wrapper = JsonConvert.DeserializeObject<FrontendModelWrapper>(input.GetText());
        if (wrapper == null)
          return null;

        int a = 0;
        return null;
      }
      catch(Exception e) { return null; }
    }

  }
}
