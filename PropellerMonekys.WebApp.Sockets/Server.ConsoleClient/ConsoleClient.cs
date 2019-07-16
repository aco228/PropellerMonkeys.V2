using PropellerMonkeys.Sockets.Device;
using PropellerMonkeys.Sockets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.WebApp.Sockets.Server.ConsoleClient
{
  public enum ConsoleClientActivity { ACTIVE, DELAY, PROBLEM, FATAL }

  public class ConsoleClient
  {
    public string UID { get; set; } = string.Empty;
    public string Username { get => this.Context != null ? this.Context.Username : string.Empty; }
    public DateTime LastPing { get; protected set; } = DateTime.Now;
    public DeviceContextModel Context { get; set; } = null;
    public ConsoleClientWrapper Wrapper
    {
      get => new ConsoleClientWrapper()
      {
        Context = this.Context,
        Activity = this.Activity,
        LastPing = this.LastPing
      };
    }
    public ConsoleClientActivity Activity
    {
      get
      {
        double secondsFromLastPing = (DateTime.Now - this.LastPing).TotalSeconds;
        if (secondsFromLastPing <= 30) return ConsoleClientActivity.ACTIVE;
        else if (secondsFromLastPing <= 50) return ConsoleClientActivity.DELAY;
        else if (secondsFromLastPing <= 120) return ConsoleClientActivity.PROBLEM;
        else return ConsoleClientActivity.FATAL;
      }
    }

    public ConsoleClient(string uid)
    {
      this.UID = uid;
      this.Context = new DeviceContextModel();
    }

    public void OnRegistration(DeviceContextModel context) => this.Context = context;

    public void OnBeforeDisconnect()
    {

    }

    public void OnPing(SocketPingModel model)
    {
      this.LastPing = DateTime.Now;
      WebSocketsMiddleware.FrontendServer.Log($"Client with username '{this.Username}' is pinging us with machineName '{this.Context.MachineName}'. Yeaa");
    }

  }
}
