using PropellerMonkeys.Sockets.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Api
{
  public class PropellerAdsApi : ApiBase
  {
    private string username = string.Empty;
    private string password = string.Empty;
    public DateTime ApiKeyExpires = DateTime.Now.AddSeconds(-30);

    public PropellerAdsApi(string username, string password) : base("Authorization", "")
    {
      this.username = username;
      this.password = password;
    }

    private void TryToLogin()
    {
      if (this.ApiKeyExpires >= DateTime.Now)
        return;

      this.TOKEN = string.Empty;
      dynamic json = this.Post(string.Format("https://ssp-api.propellerads.com/v5/adv/login?username={0}&password={1}", this.username, this.password));
      if (json == null)
        return;

      this.TOKEN = "Bearer " + json.api_token;
      this.ApiKeyExpires.AddMinutes(10);
    }

    public List<Campaign> GetCampaignsList()
    {
      List<Campaign> result = new List<Campaign>();
      foreach (var camp in this.GetCampaigns())
        result.Add(camp);
      return result;
    }
    public IEnumerable<Campaign> GetCampaigns()
    {
      this.TryToLogin();
      dynamic json = this.Get("https://ssp-api.propellerads.com/v5/adv/campaigns?page_size=1000"); // &is_archived=0
      if (json == null)
        yield break;

      foreach (var row in json.result)
      {
        Campaign campaign = PropellerAdsHelper.GetCampaignFromJson(row);
        if (campaign == null)
          continue;

        campaign.API = this;
        yield return campaign;
      }
        

      yield break;
    }
    public CampaignContext GetCurrentStats(Campaign campaign)
    {
      this.TryToLogin();
      for(; ; )
      {
        DateTime today = DateTime.Today; DateTime tomorrow = today.AddDays(2);
        dynamic json = this.Get($"https://ssp-api.propellerads.com/v5/adv/statistics/campaigns?date_from={today.Year}-{today.Month}-{today.Day}&date_to={tomorrow.Year}-{tomorrow.Month}-{tomorrow.Day}&page_size=10000&campaign_id[]=" + campaign.ID);
        if (json == null)
          continue;

        if (json.result.ToString().Equals("[]"))
          return new CampaignContext();

        CampaignContext result = new CampaignContext()
        {
          Clicks = int.Parse(json.result[0].impressions.ToString()),
          Conversions = int.Parse(json.result[0].conversions.ToString()),
          Payout = 0.0,
          Spent = double.Parse(json.result[0].money.ToString())
        };
        return result;
      }
    }

    public void StartCampaign(Campaign campaign)
    {
      this.TryToLogin();
      for (; ; )
      {
        dynamic result = this.Put("https://ssp-api.propellerads.com/v5/adv/campaigns/play", "{ \"campaign_ids\": [" + campaign.ID + "] }");
        if (result != null)
          return;
        System.Threading.Thread.Sleep(250);
      }
    }
    public bool StartCampaign(string campaignID)
    {
      this.TryToLogin();
      return this.Put("https://ssp-api.propellerads.com/v5/adv/campaigns/play", "{ \"campaign_ids\": [" + campaignID + "] }") != null;
    }
    public bool StopCampaign(Campaign campaign)
    {
      this.TryToLogin();
      for (; ; )
      {
        dynamic result = this.Put("https://ssp-api.propellerads.com/v5/adv/campaigns/stop", "{ \"campaign_ids\": [" + campaign.ID + "] }");
        if (result != null)
          return true;
        System.Threading.Thread.Sleep(250);
      }
    }
    public bool StopCampaign(string campaignID)
    {
      this.TryToLogin();
      for (; ; )
      {
        dynamic result = this.Put("https://ssp-api.propellerads.com/v5/adv/campaigns/stop", "{ \"campaign_ids\": [" + campaignID + "] }");
        if (result != null)
          return true;
        System.Threading.Thread.Sleep(250);
      }
    }

  }
}
