namespace MarsRover.Interfaces {
  public interface IUserIO {
    void WriteLine(string message);
    void Write(string message);
    string ReadLine();
    void Clear();
  }
}