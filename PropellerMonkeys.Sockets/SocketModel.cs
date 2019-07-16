using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Sockets
{
  [Serializable()]
  public class SocketModel
  {
    public virtual SocketDistributionModelType Type { get; } = SocketDistributionModelType.DEFAULT;
    public virtual SocketModelTypes SubType { get; } = SocketModelTypes.DEFAULT;
  }
}
