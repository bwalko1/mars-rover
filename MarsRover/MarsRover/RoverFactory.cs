using System;
using MarsRover.Interfaces;
using MarsRover.RoverTypes;

namespace MarsRover {
  public class RoverFactory : IRoverFactory {
    public IRover CreateRover(string type) {
      switch (type) {
        case "LandRover":
          return new LandRover();
        default:
          throw new ApplicationException($"Rover '{type}' cannot be created");
      }
    }
  }
}
