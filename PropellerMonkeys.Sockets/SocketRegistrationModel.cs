using PropellerMonkeys.Sockets.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Sockets
{
  [Serializable()]
  public class SocketRegistrationModel : SocketModel
  {
    public override SocketDistributionModelType Type => SocketDistributionModelType.LOGIN;
    public DeviceContextModel Context { get; set; } = null;

  }
}
