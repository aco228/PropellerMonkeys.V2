using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Api
{
  public static class PropellerAdsHelper
  {

    public static Campaign GetCampaignFromJson(dynamic json)
    {
      try
      {
        Campaign result = new Campaign();
        result.ID = json.id;
        result.Name = json.name;
        result.IsArchived = json.is_archived;
        result.Status = (CampaignStatus)json.status;
        return result;
      }
      catch(Exception e)
      {
        return null;
      }
    }

  }
}
