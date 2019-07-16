using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Consoles.Client.Code.Workers.Web
{
  public class BrowserReloadWorker : WorkerBase
  {
    public BrowserReloadWorker() : base("browser", "reload", 5 * 60 * 1000, false) { }

    public override void OnLoop()
    {
      Program.Browser.Driver.Navigate().Refresh();
      Program.Browser.WaitToLoad(By.ClassName("app"));
      if (Program.Browser.Driver.FindElementById("username") != null)
        Program.Browser.Login();
    }
  }
}
