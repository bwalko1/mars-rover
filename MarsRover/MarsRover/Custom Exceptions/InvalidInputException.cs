using System;

namespace MarsRover.Custom_Exceptions {
  public class InvalidInputException : Exception {
    public InvalidInputException(string message) : base(message) { }
  }
}
