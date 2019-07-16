using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace PropellerMonkeys.Api
{
  public abstract class ApiBase
  {
    protected object lock_obj = new object();
    public string TOKEN { get; protected set; } = string.Empty;
    public string HeaderInformation { get; protected set; } = string.Empty;

    public ApiBase(string headerInformations, string token)
    {
      this.TOKEN = token;
      this.HeaderInformation = headerInformations;
    }
    
    /// 
    //	WEB METHODS

    protected dynamic Get(string requestUrl)
    {
      try
      {
        var webRequest = System.Net.WebRequest.Create(requestUrl);
        if (webRequest == null)
          return null;

        webRequest.Method = "GET";
        webRequest.Timeout = 12000;
        webRequest.ContentType = "application/json";
        if (!string.IsNullOrEmpty(HeaderInformation))
          webRequest.Headers.Add(HeaderInformation, TOKEN);

        lock (this.lock_obj)
        {
          using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
          {
            using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
            {
              var jsonResponse = sr.ReadToEnd();
              dynamic json = JsonConvert.DeserializeObject(jsonResponse);

              //Console.WriteLine(String.Format("Response: {0}", jsonResponse));
              return json;
            }
          }
        }
      }
      catch (Exception ex)
      {
        return null;
      }
    }
    protected dynamic Post(string requestUrl, string data = "")
    {
      try
      {
        var webRequest = System.Net.WebRequest.Create(requestUrl);
        if (webRequest == null)
          return null;

        webRequest.Method = "POST";
        webRequest.Timeout = 12000;
        webRequest.ContentType = "application/json";
        webRequest.ContentType = "application/x-www-form-urlencoded";
        if (!string.IsNullOrEmpty(HeaderInformation) && !string.IsNullOrEmpty(this.TOKEN))
          webRequest.Headers.Add(HeaderInformation, TOKEN);

        if (!string.IsNullOrEmpty(data))
        {
          string postData = data;
          ASCIIEncoding encoding = new ASCIIEncoding();
          byte[] byte1 = encoding.GetBytes(postData);
          webRequest.ContentLength = byte1.Length;
          Stream newStream = webRequest.GetRequestStream();
          newStream.Write(byte1, 0, byte1.Length);
        }

        lock (this.lock_obj)
        {
          using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
          using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
          {
            var jsonResponse = sr.ReadToEnd();
            if (jsonResponse == "[]")
              return null;

            dynamic json = JsonConvert.DeserializeObject(jsonResponse);

            //Console.WriteLine(String.Format("Response: {0}", jsonResponse));
            return json;
          }
        }
      }
      catch (Exception ex)
      {
        //Console.WriteLine(ex.ToString());
        return null;
      }
    }
    protected dynamic Put(string requestUrl, string data)
    {
      try
      {
        var webRequest = System.Net.WebRequest.Create(requestUrl);
        if (webRequest == null)
          return null;

        webRequest.Method = "PUT";
        webRequest.Timeout = 12000;
        webRequest.ContentType = "application/json";
        webRequest.ContentType = "application/json";
        if (!string.IsNullOrEmpty(HeaderInformation) && !string.IsNullOrEmpty(this.TOKEN))
          webRequest.Headers.Add(HeaderInformation, TOKEN);

        string postData = data;
        ASCIIEncoding encoding = new ASCIIEncoding();
        byte[] byte1 = encoding.GetBytes(postData);
        webRequest.ContentLength = byte1.Length;
        Stream newStream = webRequest.GetRequestStream();
        newStream.Write(byte1, 0, byte1.Length);

        lock (this.lock_obj)
        {
          using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
          using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
          {
            var jsonResponse = sr.ReadToEnd();
            if (jsonResponse == "[]")
              return null;

            dynamic json = JsonConvert.DeserializeObject(jsonResponse);

            //Console.WriteLine(String.Format("Response: {0}", jsonResponse));
            return json;
          }
        }
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.ToString());
        return null;
      }
    }

    protected long DateToUnix(DateTime foo)
    {
      return ((DateTimeOffset)foo).ToUnixTimeSeconds();
    }
  }
}
