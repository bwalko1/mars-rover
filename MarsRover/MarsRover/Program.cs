namespace MarsRover {
  public class Program {

    private static void Main() {
      var roverFactory = new RoverFactory();
      var userIo = new ConsoleUserIO();
      var roverManager = new RoverManager(roverFactory);
      var missionControl = new MissionControl(roverManager, userIo);

      missionControl.StartMission();
    }
  }
}