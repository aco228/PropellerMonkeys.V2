using PropellerMonkeys.Sockets.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.WebApp.Sockets.Server.ConsoleClient
{
  [Serializable()]
  public class ConsoleClientWrapper
  {
    public DateTime LastPing { get; set; }
    public string SecondsElapsed { get => (int)(DateTime.Now - this.LastPing).TotalSeconds + " seconds"; }
    public ConsoleClientActivity Activity { get; set; }
    public DeviceContextModel Context { get; set; } = null;

  }
}
