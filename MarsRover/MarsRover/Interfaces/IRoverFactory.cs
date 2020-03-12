namespace MarsRover.Interfaces {
  public interface IRoverFactory {
    IRover CreateRover(string type);
  }
}
