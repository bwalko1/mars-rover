using System;

namespace MarsRover.Custom_Exceptions {
  public class RoverInTheWayException : Exception {
    public RoverInTheWayException(string message) : base(message) { }
  }
}
