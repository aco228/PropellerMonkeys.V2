using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Sockets
{
  [Serializable()]
  public class SocketJsonModel
  {
    public virtual SocketDistributionModelType Type { get; } = SocketDistributionModelType.DEFAULT;
  }
}
