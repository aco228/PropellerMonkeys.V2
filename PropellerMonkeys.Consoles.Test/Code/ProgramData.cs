using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PropellerMonkeys.Consoles.Test.Code
{
  public class ProgramData
  {
    private static string Location = @".\_DATA.txt";
    public bool HasError = false;
    public Dictionary<string, string> Data = new Dictionary<string, string>();

    public string this[string input] => Data.ContainsKey(input) ? Data[input] : string.Empty;

    public ProgramData()
    {
      if (!File.Exists(Location))
      {
        Console.WriteLine("[ERROR::] _DATA.txt file is missing!");
        this.HasError = true;
        return;
      }

      string[] lines = File.ReadAllLines(Location);

      foreach (string line in lines)
      {
        if (string.IsNullOrEmpty(line))
          continue;
        if (line.Trim()[0] == '#')
          continue;

        string withoutComment = line.Substring(0, (line.LastIndexOf('#') != -1 ? line.LastIndexOf('#') : line.Length)).Trim();
        string key = withoutComment.Substring(0, withoutComment.IndexOf(':')).Trim();
        string value = withoutComment.Substring(withoutComment.IndexOf(':') + 1, withoutComment.Length - 1 - withoutComment.IndexOf(':')).Trim();

        if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
          continue;

        if (!Data.ContainsKey(key))
          Data.Add(key, value);
      }

      this.CheckIfAllValueInformationsAreThere();

      if (string.IsNullOrEmpty(this["id"]))
      {
        Guid identifier = Guid.NewGuid();
        this.AddData("id", identifier.ToString().Split('-')[0], "Short identifier");
        this.AddData("guid", identifier.ToString(), "Identifier");
      }
    }

    public void CheckIfAllValueInformationsAreThere()
    {
      if (string.IsNullOrEmpty(this["username"]))
      {
        Console.WriteLine("[ERROR::] Username is not listed into _data file");
        this.HasError = true;
      }

      if (string.IsNullOrEmpty(this["password"]))
      {
        Console.WriteLine("[ERROR::] Password is not listed into _data file");
        this.HasError = true;
      }

    }

    public void AddData(string key, string value, string description = "")
    {
      if (Data.ContainsKey(key))
      {
        UpdateData(key, value, description);
        return;
      }

      if (!string.IsNullOrEmpty(description) && description[0] != '#')
        description = "   #" + description;
      File.AppendAllText(Location, Environment.NewLine + string.Format("{0}:{1}{2}", key, value, description));
      this.Data.Add(key, value);
    }

    public void UpdateData(string key, string value, string description = "")
    {
      if (!string.IsNullOrEmpty(description) && description[0] != '#')
        description = "   #" + description;

      string[] lines = File.ReadAllLines(Location);
      List<string> newLines = new List<string>();
      foreach (string l in lines)
      {
        string line = l.Trim();
        if (line.StartsWith(key))
          line = string.Format("{0}:{1}{2}", key, value, description);
        newLines.Add(line);
      }

      File.WriteAllLines(Location, newLines.ToArray());
      this.Data[key] = value;
    }
  }
}
