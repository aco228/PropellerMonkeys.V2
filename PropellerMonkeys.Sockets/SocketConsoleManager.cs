using System;

namespace PropellerMonkeys.Sockets
{
  public class SocketConsoleManager
  {
    public Action<string> OnNewLine = null;

    public void WriteLine(string line)
    {
      Console.WriteLine(line);
      OnNewLine?.Invoke(line);
    }

  }
}
