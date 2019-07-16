using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PropellerMonkeys.Consoles.Client.Code.Workers
{
  public abstract class WorkerBase
  {
    public string PrimaryKey { get; private set; } = string.Empty;
    public string SecondaryKey { get; private set; } = string.Empty;
    public int Timeout { get; protected set; } = 0;
    public bool Running { get; protected set; } = false;
    public bool StartImmediatly { get; protected set; } = false;
    public DateTime LastExecution { get; protected set; } = DateTime.Now;
    public DateTime? WaitUntilDate { get; set; } = null;
    public Thread Thead = null;

    public WorkerBase(string primaryKey, string secondaryKey, int timeout, bool startImmediatly = false)
    {
      this.PrimaryKey = primaryKey;
      this.SecondaryKey = secondaryKey;
      this.Timeout = timeout;
      this.StartImmediatly = startImmediatly;

      this.Start();
    }

    //
    // Abstract methods
    //

    public abstract void OnLoop();

    //
    // Public methods
    //

    public void Start()
    {
      if (this.Running)
        return;
      this.Running = true;

      this.Thead = new Thread(() =>
      {
        for (; ; )
        {
          if (!this.StartImmediatly)
            Thread.Sleep(this.Timeout);

          if (this.WaitUntilDate.HasValue && this.WaitUntilDate > DateTime.Now)
            continue;

          if (this.WaitUntilDate.HasValue)
            this.WaitUntilDate = null;


          try
          {
            this.OnLoop();
          }
          catch (Exception e)
          {
            Console.WriteLine($"Worker.{this.PrimaryKey}.{this.SecondaryKey} EXCEPTION:: " + e.ToString());
          }

          this.LastExecution = DateTime.Now;

          if (this.StartImmediatly)
            Thread.Sleep(this.Timeout);
        }
      });
      this.Thead.Start();
    }

  }
}
