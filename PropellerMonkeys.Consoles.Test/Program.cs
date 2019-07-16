using PropellerMonkeys.Api;
using PropellerMonkeys.Consoles.Client.Code.Web;
using PropellerMonkeys.Consoles.Test.Code;
using PropellerMonkeys.Consoles.Test.Code.Workers;
using PropellerMonkeys.Sockets.Device;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Threading.Tasks;
using WebSocketSharp;

namespace PropellerMonkeys.Consoles.Client
{
  class Program
  {
    public static ProgramData Data = null;
    public static WorkerManager WorkerManager = null;
    public static MainBrowser Browser = null;
    public static ClientSocket Socket = null;
    public static PropellerAdsApi API = null;
    public static ClientContext Context = null;
    

    private static void Main(string[] args)
    {
      Console.WriteLine("Starting application");
      Data = new ProgramData();
      if (Data.HasError)
      {
        Console.ReadKey();
        return;
      }

      Socket = new ClientSocket();
      WorkerManager = new WorkerManager();
      Browser = new MainBrowser();
      API = new PropellerAdsApi(Data["username"], Data["password"]);
      Context = new ClientContext();

      Console.WriteLine("Application has started...");
      Console.ReadKey();
    }


    
  }
}
