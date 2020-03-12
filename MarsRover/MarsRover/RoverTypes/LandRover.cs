using System.Collections.Generic;
using MarsRover.Interfaces;

namespace MarsRover.RoverTypes {
  public class LandRover : IRover {
    public int XCoord { get; set; }
    public int YCoord { get; set; }
    public char Direction { get; set; }
    public int RoverId { get; set; }

    private readonly Dictionary<char, char> _turnRight = new Dictionary<char, char> {
      { 'N', 'E' },
      { 'E', 'S' },
      { 'S', 'W' },
      { 'W', 'N' }
    };

    private readonly Dictionary<char, char> _turnLeft = new Dictionary<char, char> {
      { 'N', 'W' },
      { 'W', 'S' },
      { 'S', 'E' },
      { 'E', 'N' }
    };

    public LandRover() { }

    public LandRover(int xCoord, int yCoord, int roverId, char direction) {
      XCoord = xCoord;
      YCoord = yCoord;
      RoverId = roverId;
      Direction = direction;
    }

    public void SetRoverValues(int xCoord, int yCoord, int roverId, char direction) {
      XCoord = xCoord;
      YCoord = yCoord;
      RoverId = roverId;
      Direction = direction;
    }

    public string GetStatus() {
      return $"Rover {RoverId} is at {XCoord}, {YCoord} facing {Direction}.";
    }

    public int[] GetCoords() {
      return new[] { XCoord, YCoord };
    }

    public void TurnRight() {
      Direction = _turnRight[Direction];
    }

    public void TurnLeft() {
      Direction = _turnLeft[Direction];
    }

    public void MoveForward() {
      if (Direction == 'N') {
        YCoord++;
      } else if (Direction == 'W') {
        XCoord--;
      } else if (Direction == 'S') {
        YCoord--;
      } else if (Direction == 'E') {
        XCoord++;
      }
    }
  }
}