using PropellerMonkeys.Sockets.ServerClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Consoles.Client
{
  public class ClientSocket : ServerClientBase
  {
    public ClientSocket() : base("ws://localhost:52613/ws_clientconsole") { }
  }
}
