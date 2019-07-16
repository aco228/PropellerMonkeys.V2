using PropellerMonkeys.Api;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Consoles.Client
{
  public class CampaignContext
  {
    public string Name { get; set; } = string.Empty;
    public List<Campaign> ActiveCampaigns { get; set; } = new List<Campaign>();
    public List<Campaign> ArchivedCampaigns { get; set; } = new List<Campaign>();
  }
}
