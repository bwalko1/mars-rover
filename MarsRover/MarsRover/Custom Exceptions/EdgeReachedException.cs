using System;

namespace MarsRover.Custom_Exceptions {
  public class EdgeReachedException : Exception {
    public EdgeReachedException(string message) : base(message) { }
  }
}
