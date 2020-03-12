using System.Collections.Generic;
using System.Text.RegularExpressions;
using MarsRover.Custom_Exceptions;
using MarsRover.Interfaces;

namespace MarsRover {
  public class MissionControl {
    private const int MAX_GRID_SIZE = 256;
    private const int MAX_GRID_INPUT_LENGTH = 2;
    private const int MAX_ROVER_INPUT_LENGTH = 3;

    private readonly IRoverManager _roverManager;
    private readonly IUserIO _userIo;
    private readonly List<string> _validDirections = new List<string> { "N", "S", "W", "E" };

    public int GridWidth { get; set; }
    public int GridHeight { get; set; }

    public MissionControl(IRoverManager roverManager, IUserIO userIo) {
      _roverManager = roverManager;
      _userIo = userIo;
    }

    public void StartMission() {
      GreetingMessage();
      GridSetup();
      MissionLoop();
    }

    public void GridSetup() {
      while (true) {
        try {
          _userIo.WriteLine("What size would you like to make the grid?\n" +
                            $"Max size: {MAX_GRID_SIZE}x{MAX_GRID_SIZE} (Ex. '3 4' makes a 3x4 grid.)");
          var input = GetUserInput().Split(' ');

          if (input.Length != MAX_GRID_INPUT_LENGTH || !IsValidGridInput(input)) {
            throw new InvalidInputException("Invalid grid size input.");
          }

          if (IsValidGridSize(input)) {
            throw new InvalidInputException($"Invalid grid size input, please use a valid grid size 0 < Length/Width < {MAX_GRID_SIZE}.");
          }

          GridWidth = int.Parse(input[0]);
          GridHeight = int.Parse(input[1]);

          ClearAndWriteLine($"Grid was initialized with size {GridWidth}x{GridHeight}");

          break;
        }
        catch (InvalidInputException invalidInputException) {
          ClearAndWriteLine(invalidInputException.Message);
        }
      }
    }

    public void MissionLoop() {
      while (true) {
        try {
          OutputMainMenu();
          var input = GetUserInput();
          if (input == "4") {
            break;
          }

          ExecuteMainMenuInput(input);
        }
        catch (InvalidInputException invalidInputException) {
          ClearAndWriteLine(invalidInputException.Message);
        }
        catch (EmptyRoverListException emptyRoverListException) {
          ClearAndWriteLine(emptyRoverListException.Message);
        }
      }
    }

    public void ExecuteMainMenuInput(string input) {
      switch (input) {
        case "1":
          LaunchRoverMenu();
          break;
        case "2":
          DriveRoverMenu();
          break;
        case "3":
          OutputRoverStatus();
          break;
        default:
          throw new InvalidInputException("Invalid input, please try again.");
      }
    }

    public void LaunchRoverMenu() {
      _userIo.Clear();
      while (true) {
        try {
          _userIo.WriteLine("Where would you like to place the new rover?\n" +
                            "Syntax: 'X Y Direction' ('E' to exit)");
          var input = GetUserInput().ToUpper().Split(' ');

          if (input[0] == "E") {
            break;
          }

          if (input.Length != MAX_ROVER_INPUT_LENGTH || !IsValidRoverInput(input)) {
            throw new InvalidInputException("Invalid input, please try again.");
          }

          if (!IsValidRoverCoords(input)) {
            throw new RoverInTheWayException("Cannot place a rover here, space occupied.");
          }

          if (!CheckGridEdge(new[] { int.Parse(input[0]), int.Parse(input[1]) })) {
            throw new EdgeReachedException("Cannot place a rover outside of grid.");
          }

          LaunchRover(input);
          break;
        }
        catch (InvalidInputException invalidInputException) {
          ClearAndWriteLine(invalidInputException.Message);
        }
        catch (RoverInTheWayException roverInTheWayException) {
          ClearAndWriteLine(roverInTheWayException.Message);
        }
        catch (EdgeReachedException edgeReachedException) {
          ClearAndWriteLine(edgeReachedException.Message);
        }
      }

      ClearAndWriteLine("Rover launched successfully.");
    }

    public void DriveRoverMenu() {
      if (_roverManager.AllRoverStatus().Length == 0) {
        throw new EmptyRoverListException("There are no rover's to drive.");
      }

      _userIo.Clear();

      while (true) {
        try {
          _userIo.WriteLine(CurrentRover().GetStatus());
          _userIo.WriteLine("M to move forward\n" +
                            "L to turn left\n" +
                            "R to turn right\n" +
                            "E to exit");
          var input = GetUserInput().ToUpper();
          if (input == "E") {
            break;
          }

          if (input.Length == 0) {
            throw new InvalidInputException("Invalid input, please try again.");
          }

          ExecuteDriveRoverInput(input);
          _userIo.Clear();
        }
        catch (InvalidInputException invalidInputException) {
          ClearAndWriteLine(invalidInputException.Message);
        }
        catch (EdgeReachedException edgeReachedException) {
          ClearAndWriteLine(edgeReachedException.Message);
        }
        catch (RoverInTheWayException roverInTheWayException) {
          ClearAndWriteLine(roverInTheWayException.Message);
        }
      }

      _userIo.Clear();
    }

    public bool IsValidMove(int[] futureCoords, char currentDirection) {
      if (currentDirection == 'N') {
        futureCoords[1]++;
      } else if (currentDirection == 'W') {
        futureCoords[0]--;
      } else if (currentDirection == 'S') {
        futureCoords[1]--;
      } else if (currentDirection == 'E') {
        futureCoords[0]++;
      }

      return !_roverManager.CheckCoords(futureCoords);
    }

    public bool CheckGridEdge(IReadOnlyList<int> futureCoords) {
      return futureCoords[0] >= 0 && futureCoords[0] <= GridWidth && futureCoords[1] >= 0 && futureCoords[1] <= GridHeight;
    }

    public void OutputRoverStatus() {
      if (_roverManager.RoverCount == 0) {
        throw new EmptyRoverListException("There are no rover's to output.");
      }

      ClearAndWriteLine(_roverManager.AllRoverStatus());
    }

    public void ExecuteDriveRoverInput(string input) {
      while (true) {
        switch (input[0]) {
          case 'M':
            var futureCoords = CurrentRover().GetCoords();
            var currentDirection = CurrentRover().Direction;

            if (!IsValidMove(futureCoords, currentDirection)) {
              throw new RoverInTheWayException("Cannot move forward, there is a rover in the way.");
            }

            if (!CheckGridEdge(futureCoords)) {
              throw new EdgeReachedException("You have reached the edge. Turn around.");
            }

            CurrentRover().MoveForward();
            break;
          case 'L':
            CurrentRover().TurnLeft();
            break;
          case 'R':
            CurrentRover().TurnRight();
            break;
          default:
            throw new InvalidInputException("Invalid input, please try again.");
        }

        input = input.Substring(1, input.Length - 1);
        if (input.Length == 0) {
          break;
        }
      }
    }

    public void GreetingMessage() {
      _userIo.WriteLine("Welcome NASA's Mars Rover Initiative.");
    }

    public void OutputMainMenu() {
      _userIo.WriteLine("____MAIN MENU____\n" +
                        "[1] Launch Rover\n" +
                        "[2] Drive Rover\n" +
                        "[3] Rover Status\n" +
                        "[4] Exit");
    }

    public bool IsValidRoverCoords(IReadOnlyList<string> input) {
      return !_roverManager.CheckCoords(new[] { int.Parse(input[0]), int.Parse(input[1]) });
    }

    public bool IsValidRoverInput(IReadOnlyList<string> input) {
      return IsNumber(input[0]) && IsNumber(input[1]) && _validDirections.Contains(input[2]);
    }

    public void LaunchRover(IReadOnlyList<string> input) {
      _roverManager.CreateRover(int.Parse(input[0]), int.Parse(input[1]), char.Parse(input[2]));
    }

    public static bool IsValidGridSize(IReadOnlyList<string> input) {
      return int.Parse(input[0]) > MAX_GRID_SIZE || int.Parse(input[1]) > MAX_GRID_SIZE;
    }

    public static bool IsValidGridInput(IReadOnlyList<string> input) {
      return IsNumber(input[0]) && IsNumber(input[1]);
    }

    public static bool IsNumber(string input) {
      return Regex.IsMatch(input, @"^\d+$");
    }

    public string GetUserInput() {
      _userIo.Write(": ");
      return _userIo.ReadLine();
    }

    public IRover CurrentRover() {
      return _roverManager.CurrentRover();
    }

    public void ClearAndWriteLine(string message) {
      _userIo.Clear();
      _userIo.WriteLine(message);
    }
  }
}