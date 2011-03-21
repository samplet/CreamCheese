using System;
using System.IO;

namespace SpreadSim {

  public static class SpreadSim {

    public static void
    Main(string[] args) {
      Stream stream;
      try {
        if (args.Length > 0) {
          stream = new FileStream(args[0], FileMode.Open);
        } else {
          stream = Console.OpenStandardInput();
        }
        CommandParser.Parser parser = new CommandParser.Parser(stream);
        parser.Parse();
      }
      catch(Exception e) {
        Console.Error.WriteLine("SpreadSim: " + e.Message);
      }
    }

  }

}
