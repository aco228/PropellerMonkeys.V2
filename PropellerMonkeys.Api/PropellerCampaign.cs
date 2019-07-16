using PropellerMonkeys.Sockets.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Api
{

  public enum CampaignStatus
  {
    Default,
    Draft = 1,
    ModerationPending = 2,
    Rejected = 3,
    Ready = 4,
    TestRun = 5,
    Working = 6,
    Paused = 7,
    Stopped = 8,
    Completed = 9
  }


  public class Campaign
  {
    public int ID { get; set; } = -1;
    public string Name { get; set; } = string.Empty;
    public bool IsArchived { get; set; } = false;
    public CampaignStatus Status { get; set; } = CampaignStatus.Default;
    public bool IsCampaignActive { get => this.Status == CampaignStatus.Working; }
    public PropellerAdsApi API { get; set; } = null;

    public void Start()
    {
      if (API != null)
        API.StartCampaign(this);
    }

    public void Stop()
    {
      if (API != null)
        API.StopCampaign(this);
    }

    public CampaignContext GetStats()
    {
      if (API != null)
        return API.GetCurrentStats(this);
      return null;
    }
  }
}
