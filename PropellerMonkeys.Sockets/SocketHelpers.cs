using System;
using System.Collections.Generic;
using System.Text;

namespace PropellerMonkeys.Sockets
{
  public static class SocketHelpers
  {
    public static SocketDistributionModel Instantiate(this SocketModel input) => new SocketDistributionModel(input, input.Type, SocketDistributionModelDataType.BINARY);
    public static SocketDistributionModel Instantiate(this SocketJsonModel input) => new SocketDistributionModel(input, input.Type, SocketDistributionModelDataType.JSON);
  }
}
