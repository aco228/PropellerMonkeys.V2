using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Sockets.Device
{
  [Serializable()]
  public class DeviceContextModel : SocketModel
  {
    public override SocketModelTypes SubType => base.SubType;

    public string Username { get; set; } = string.Empty;
    public string MachineName { get; set; } = string.Empty;
    public double CurrentBalance { get; set; } = 0.0;
  }
}
