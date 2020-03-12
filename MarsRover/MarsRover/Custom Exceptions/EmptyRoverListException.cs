using System;

namespace MarsRover.Custom_Exceptions {
  public class EmptyRoverListException : Exception {
    public EmptyRoverListException(string message) : base(message) { }
  }
}
