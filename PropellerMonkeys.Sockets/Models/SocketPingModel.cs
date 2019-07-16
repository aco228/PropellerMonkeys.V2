using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Sockets.Models
{
  [Serializable()]
  public class SocketPingModel : SocketModel
  {
    public override SocketModelTypes SubType => SocketModelTypes.PING;
    
  }
}
