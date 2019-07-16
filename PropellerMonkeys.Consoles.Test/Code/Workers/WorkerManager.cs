using PropellerMonkeys.Consoles.Client.Code.Workers;
using PropellerMonkeys.Consoles.Client.Code.Workers.Sockets;
using PropellerMonkeys.Consoles.Client.Code.Workers.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Consoles.Test.Code.Workers
{
  public class WorkerManager
  {
    public static Dictionary<string, Dictionary<string, WorkerBase>> Threads { get; set; } = null; // Primary Key _ Secondary Key _ Thread

    public WorkerManager()
    {
      new PingWorker();

      new BrowserReloadWorker();
  }

    public void Add(string primaryKey, string secondaryKey, WorkerBase worker)
    {
      if (Threads == null)
        Threads = new Dictionary<string, Dictionary<string, WorkerBase>>();
      if (!Threads.ContainsKey(primaryKey))
        Threads.Add(primaryKey, new Dictionary<string, WorkerBase>());
      if (!Threads[primaryKey].ContainsKey(secondaryKey))
        Threads[primaryKey].Add(secondaryKey, worker);
    }

  }
}
