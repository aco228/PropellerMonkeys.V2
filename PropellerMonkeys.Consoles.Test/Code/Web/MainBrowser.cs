using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Consoles.Client.Code.Web
{
  public class MainBrowser : Browser
  {
    public MainBrowser()
    {
      this.Login();
    }

    public void Login()
    {
      LoadPage("https://partners.propellerads.com/#/app/auth", By.Id("username"));
      Driver.FindElementById("username").SendKeys(Program.Data["username"]);
      Driver.FindElementById("password").SendKeys(Program.Data["password"]);
      Driver.FindElementsByClassName("button_primary")[0].Click();
      Driver.Navigate().GoToUrl("https://partners.propellerads.com/#/app/campaigns");
      GetCampaigns();
    }

    public string GetCurrentBalance()
    {
      try
      {
        Driver.Navigate().Refresh();
        WaitToLoad(By.ClassName("balance-widget__value"));
        return Driver.FindElementByClassName("balance-widget__value").Text;
      }
      catch (Exception e)
      {
        return string.Empty;
      }
    }

    public void GetCampaigns()
    {
      Console.WriteLine("Waiting for campaings..");
      WaitToLoad(By.ClassName("table__body"));

      List<CampaignContext> campaigns = new List<CampaignContext>();

      var tableBody = Driver.FindElementByClassName("table__body");
      foreach(var element in tableBody.FindElements(By.ClassName("table__row")))
      {
        var cells = element.FindElements(By.ClassName("table__cell"));
        string status = cells[1].FindElement(By.ClassName("CampaignStatus__text_desktop")).Text;
        string id = cells[2].Text;
        string name = cells[3].FindElement(By.ClassName("campaigns__view")).Text;
        string startDate = cells[4].Text;
        //string impressions = cells[5]

        //https://partners.propellerads.com/v1.0/advertiser/campaigns/?dateFrom=2019-07-06&dateTill=2019-07-06&isArchived=0&orderBy=id&orderDest=desc&page=1&perPage=100

        Console.WriteLine(cells[3].FindElement(By.ClassName("campaigns__view")).Text);
      }
    }

  }
}
