using System;
using System.Collections.Generic;
using System.Text;
using PropellerMonkeys.Sockets;

namespace PropellerMonkeys.WebApp.Sockets.Server.Frontend.Models.Send
{
  [Serializable()]
  public class FrontendConsoleModel : FrontendSendingWrapper
  {
    public override string subtype => "console";
    public override string action => "console";

    public string text { get; set; } = string.Empty;
    public string created { get; set; } = string.Empty;

    public FrontendConsoleModel(string text)
    {
      this.text = text;
      DateTime now = DateTime.Now;
      this.created = string.Format("{0}:{1}.{2}",
        (now.Hour < 10 ? "0" : "") + now.Hour,
        (now.Minute < 10 ? "0" : "") + now.Minute,
        (now.Second < 10 ? "0" : "") + now.Second);
    }

  }
}
