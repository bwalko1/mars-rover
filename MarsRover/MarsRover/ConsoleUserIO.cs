using System;
using MarsRover.Interfaces;

namespace MarsRover {
  public class ConsoleUserIO : IUserIO {
    public void WriteLine(string message) {
      Console.WriteLine(message);
    }

    public void Write(string message) {
      Console.Write(message);
    }

    public string ReadLine() {
      return Console.ReadLine();
    }

    public void Clear() {
      Console.Clear();
    }
  }
}
