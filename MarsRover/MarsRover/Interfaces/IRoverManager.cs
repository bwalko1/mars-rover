namespace MarsRover.Interfaces {
  public interface IRoverManager {
    int RoverCount { get; }
    void CreateRover(int xCoordIn, int yCoordIn, char directionIn);
    string AllRoverStatus();
    bool CheckCoords(int[] coords);
    IRover CurrentRover();
  }
}