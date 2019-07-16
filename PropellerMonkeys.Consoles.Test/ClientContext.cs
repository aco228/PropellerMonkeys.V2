using PropellerMonkeys.Api;
using PropellerMonkeys.Sockets.Device;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Consoles.Client
{
  public class ClientContext
  {
    public DeviceContextModel ContextModel { get; set; } = null;
    public List<CampaignContext> Campaigns = new List<CampaignContext>();

    public ClientContext()
    {
      ContextModel = new DeviceContextModel()
      {
        Username = Program.Data["username"],
        MachineName = Environment.MachineName
      };

      Program.Socket.Register(this.ContextModel);

      /*
      foreach(var camp in Program.API.GetCampaigns())
      {
        if (camp.IsCampaignActive) camp.Stop();
        var campContext = new CampaignContext();
        campContext.ActiveCampaigns.Add(camp);
        Campaigns.Add(campContext);
      }
      */

    }



  }
}
