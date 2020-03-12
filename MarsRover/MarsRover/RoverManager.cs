using System.Collections.Generic;
using MarsRover.Interfaces;

namespace MarsRover {
  public class RoverManager : IRoverManager {
    private readonly IRoverFactory _roverFactory;
    private const string ROVERTYPE = "LandRover";

    public int RoverCount { get; private set; }
    public List<IRover> RoverList;

    public RoverManager(IRoverFactory iRoverFactory) {
      _roverFactory = iRoverFactory;
      RoverList = new List<IRover>();
    }

    public void CreateRover(int xCoordIn, int yCoordIn, char directionIn) {
      RoverList.Add(_roverFactory.CreateRover(ROVERTYPE));
      CurrentRover().SetRoverValues(xCoordIn, yCoordIn, RoverCount, directionIn);
      RoverCount++;
    }

    public string AllRoverStatus() {
      var roverStatuses = "";
      foreach (var rover in RoverList) {
        roverStatuses += rover.GetStatus() + '\n';
      }

      return roverStatuses;
    }

    public bool CheckCoords(int[] coords) {
      foreach (var rover in RoverList) {
        if (rover.GetCoords()[0] == coords[0] && rover.GetCoords()[1] == coords[1]) {
          return true;
        }
      }
      return false;
    }

    public IRover CurrentRover() {
      return RoverList[RoverList.Count - 1];
    }
  }
}