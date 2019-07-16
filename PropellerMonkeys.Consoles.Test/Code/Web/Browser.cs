using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PropellerMonkeys.Consoles.Client.Code.Web
{
  public abstract class Browser
  {
    public ChromeDriver Driver { get; protected set; } = null;

    public Browser()
    {
      ChromeOptions options = new ChromeOptions();
      options.AddArguments("--incognito");
      //options.AddArgument("--headless");
      this.Driver = new ChromeDriver(@".\", options);
    }

    public void LoadPage(string url, By byElement = null)
    {
      this.Driver.Navigate().GoToUrl(url);
      if (byElement == null)
        return;

      WaitToLoad(byElement);
      return;
    }

    public bool WaitToLoad(By by)
    {
      int i = 0;
      while (i < 600)
      {
        i++;
        Thread.Sleep(100); // sleep 100 ms
        try
        {
          this.Driver.FindElement(by);
          break;
        }
        catch { }
      }
      if (i == 1200) return false; // page load failed in 1 min
      else return true;
    }

  }
}
