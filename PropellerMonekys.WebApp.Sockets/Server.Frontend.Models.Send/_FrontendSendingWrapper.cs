using PropellerMonkeys.Sockets;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.WebApp.Sockets.Server.Frontend.Models.Send
{
  [Serializable()]
  public class FrontendSendingWrapper : SocketJsonModel
  {
    public virtual string subtype { get; } = string.Empty;
    public virtual string action { get; } = string.Empty;
  }
}
