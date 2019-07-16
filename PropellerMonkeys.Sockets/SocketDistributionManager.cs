using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace PropellerMonkeys.Sockets
{
  public static class SocketDistributionManager
  {

    public static byte[] GetBytesFromObj(object data)
    {
      var binFormatter = new BinaryFormatter();
      var mStream = new MemoryStream();
      binFormatter.Serialize(mStream, data);
      return mStream.ToArray();
    }

    public static byte[] GetBytesFromJsonObj(object data)
    {
      string result = Newtonsoft.Json.JsonConvert.SerializeObject(data);
      return Encoding.UTF8.GetBytes(result);
    }

    public static object GetObjFromBytes(byte[] data)
    {
      var mStream = new MemoryStream();
      var binFormatter = new BinaryFormatter();
      mStream.Write(data, 0, data.Length);
      mStream.Position = 0;
      return binFormatter.Deserialize(mStream);
    }

    public static SocketDistributionModel ConvertToModel(byte[] data)
    {
      var mStream = new MemoryStream();
      var binFormatter = new BinaryFormatter();
      mStream.Write(data, 0, data.Length);
      mStream.Position = 0;
      return binFormatter.Deserialize(mStream) as SocketDistributionModel;
    }

    public static T Convert<T>(this SocketDistributionModel input)
    {
      var mStream = new MemoryStream();
      var binFormatter = new BinaryFormatter();
      mStream.Write(input.Data, 0, input.Data.Length);
      mStream.Position = 0;
      return (T)binFormatter.Deserialize(mStream);
    }

  }
}
