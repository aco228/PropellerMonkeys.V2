using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace PropellerMonkeys.Sockets
{
  public enum SocketDistributionModelType { DEFAULT, INTERNAL, LOGIN } 
  public enum SocketDistributionModelDataType { BINARY, JSON }

  [Serializable()]
  public class SocketDistributionModel
  {
    public SocketDistributionModelType Type { get; set; } = SocketDistributionModelType.DEFAULT;
    public virtual SocketDistributionModelDataType DataType { get; } = SocketDistributionModelDataType.BINARY;
    public WebSocketMessageType WebSocketMessageType => this.DataType == SocketDistributionModelDataType.BINARY ? WebSocketMessageType.Binary : WebSocketMessageType.Text;
    public byte[] Data { get; set; } = null;

    public SocketDistributionModel(object data, SocketDistributionModelType type, SocketDistributionModelDataType dataType)
    {
      this.DataType = dataType;
      this.Type = type;
      this.PrepareData(data);
    }

    public SocketDistributionModel(object data, SocketDistributionModelType type)
    {
      this.Type = type;
      this.PrepareData(data);
    }

    public SocketDistributionModel(object data) => this.PrepareData(data);

    private void PrepareData(object data)
    {
      if (DataType == SocketDistributionModelDataType.BINARY)
        Data = SocketDistributionManager.GetBytesFromObj(data);
      else if (DataType == SocketDistributionModelDataType.JSON)
        Data = SocketDistributionManager.GetBytesFromJsonObj(data);
    }

  }
}
