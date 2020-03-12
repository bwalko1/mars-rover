namespace MarsRover.Interfaces {
  public interface IRover {
    int XCoord { get; set; }
    int YCoord { get; set; }
    int RoverId { get; set; }
    char Direction { get; set; }

    void SetRoverValues(int xCoord, int yCoord, int id, char direction);
    string GetStatus();
    int[] GetCoords();
    void TurnRight();
    void TurnLeft();
    void MoveForward();
  }
}