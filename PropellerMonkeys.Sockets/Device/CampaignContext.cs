using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Sockets.Device
{
  [Serializable()]
  public class CampaignContext
  {
    public string CampaignID { get; set; } = string.Empty;
    public double Spent { get; set; } = 0.0;
    public double Payout { get; set; } = 0.0;
    public double CPA { get => Spent == 0 || Conversions == 0 ? 0.0 : (Spent / (Conversions * 1.0)); }
    public int Clicks { get; set; } = 0;
    public int Conversions { get; set; } = 0;
  }
}
