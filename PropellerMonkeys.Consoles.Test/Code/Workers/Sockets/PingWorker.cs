using PropellerMonkeys.Sockets.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Consoles.Client.Code.Workers.Sockets
{
  public class PingWorker : WorkerBase
  {
    public PingWorker() : base("sockets", "ping", 15000)
    {
    }

    public override void OnLoop()
    {
      Console.WriteLine("socket ping");
      Program.Socket.Send(new SocketPingModel(){ });
    }
  }
}
