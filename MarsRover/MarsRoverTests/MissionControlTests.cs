using MarsRover;
using MarsRover.Custom_Exceptions;
using MarsRover.Interfaces;
using MarsRover.RoverTypes;
using NUnit.Framework;
using Rhino.Mocks;

namespace MarsRoverTests {
  [TestFixture]
  public class MissionControlTests {
    private MissionControl _missionControl;

    private IRoverManager _mockRoverManager;
    private IUserIO _mockUserIo;

    [SetUp]
    public void SetUp() {
      _mockRoverManager = MockRepository.GenerateMock<IRoverManager>();
      _mockUserIo = MockRepository.GenerateMock<IUserIO>();
    }

    [Test]
    public void GreetingMessagePrintsCorrectly() {
      _missionControl = new MissionControl(null, _mockUserIo);
      _missionControl.GreetingMessage();
      _mockUserIo.AssertWasCalled(x => x.WriteLine("Welcome NASA's Mars Rover Initiative."));
    }

    [Test]
    public void MainMenuPrintsCorrectly() {
      _missionControl = new MissionControl(null, _mockUserIo);
      _missionControl.OutputMainMenu();
      _mockUserIo.AssertWasCalled(x => x.WriteLine("____MAIN MENU____\n" +
                                                   "[1] Launch Rover\n" +
                                                   "[2] Drive Rover\n" +
                                                   "[3] Rover Status\n" +
                                                   "[4] Exit"));
    }

    [Test]
    [TestCase(new[] { 1, 1 }, 'N')]
    [TestCase(new[] { 1, 1 }, 'S')]
    [TestCase(new[] { 1, 1 }, 'E')]
    [TestCase(new[] { 1, 1 }, 'W')]
    public void IsValidMoveReturnsTrueIfValidMove(int[] futureCoords, char currentDirection) {
      _missionControl = new MissionControl(_mockRoverManager, null);
      Assert.IsTrue(_missionControl.IsValidMove(futureCoords, currentDirection));
    }

    [Test]
    [TestCase(new[] { 1, 1 }, 4, 4, true)]
    [TestCase(new[] { 6, 1 }, 5, 5, false)]
    [TestCase(new[] { 1, 12 }, 5, 13, true)]
    [TestCase(new[] { 20, 1 }, 19, 5, false)]
    public void CheckGridEdgeReturnsTrueIfCoordInGridAndFalseIfNot(int[] futureCoords, int gridWidth, int gridHeight, bool expected) {
      _missionControl = new MissionControl(_mockRoverManager, null) {
        GridWidth = gridWidth,
        GridHeight = gridHeight
      };

      Assert.AreEqual(expected, _missionControl.CheckGridEdge(futureCoords));
    }

    [Test]
    [TestCase(new[] { "0", "0" }, false)]
    [TestCase(new[] { "0", "1" }, true)]
    public void IsValidRoverCoordsReturnsTrueIfCoordsValidAndFalseIfNot(string[] input, bool expected) {
      _missionControl = new MissionControl(_mockRoverManager, null);
      _mockRoverManager.Stub(x => x.CreateRover(0, 0, 'N'));
      _mockRoverManager.Stub(x => x.CheckCoords(new[] { int.Parse(input[0]), int.Parse(input[1]) })).Return(!expected);

      Assert.AreEqual(expected, _missionControl.IsValidRoverCoords(input));
    }

    [Test]
    [TestCase(new[] { "0", "1", "N" }, true)]
    [TestCase(new[] { "64", "1", "E" }, true)]
    [TestCase(new[] { "a", "b", "1" }, false)]
    [TestCase(new[] { "1", "b", "S" }, false)]
    [TestCase(new[] { "100", "8", "j" }, false)]
    [TestCase(new[] { "N", "S", "W" }, false)]
    public void IsValidRoverInputReturnsTrueIfInputIsValidAndFalseIfNot(string[] input, bool expected) {
      _missionControl = new MissionControl(null, null);

      Assert.AreEqual(expected, _missionControl.IsValidRoverInput(input));
    }

    [Test]
    public void LaunchRoverCreatesNewRover() {
      _missionControl = new MissionControl(_mockRoverManager, null);
      _mockRoverManager.Stub(x => x.CreateRover(0, 0, 'N'));
      _mockRoverManager.Stub(x => x.CurrentRover()).Return(new LandRover());

      _missionControl.LaunchRover(new[] { "0", "0", "N" });
      _mockRoverManager.AssertWasCalled(x => x.CreateRover(0, 0, 'N'));

      Assert.IsInstanceOf<IRover>(_mockRoverManager.CurrentRover());
    }

    [Test]
    [TestCase("test", "test")]
    [TestCase("4 4", "4 4")]
    [TestCase("1", "1")]
    [TestCase("E", "E")]
    public void GetUserInputCallsWriteAndReadLine(string input, string expected) {
      _missionControl = new MissionControl(null, _mockUserIo);
      _mockUserIo.Stub(x => x.ReadLine()).Return(input);

      var actual = _missionControl.GetUserInput();
      _mockUserIo.AssertWasCalled(x => x.Write(": "));

      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void CurrentRoverReturnsAnInstanceOfTypeIRover() {
      _missionControl = new MissionControl(_mockRoverManager, null);
      _mockRoverManager.Stub(x => x.CurrentRover()).Return(new LandRover());

      Assert.IsInstanceOf<IRover>(_missionControl.CurrentRover());
    }

    [Test]
    [TestCase("input")]
    [TestCase("message")]
    [TestCase("exception message")]
    public void ClearAndWriteLineCallsConsoleClearAndWriteLinePutsOutMessage(string message) {
      _missionControl = new MissionControl(null, _mockUserIo);
      _missionControl.ClearAndWriteLine(message);

      _mockUserIo.AssertWasCalled(x => x.Clear());
      _mockUserIo.AssertWasCalled(x => x.WriteLine(message));
    }

    [Test]
    public void OutputRoverStatusCallsClearAndWriteLineOnAllRoverStatus() {
      _missionControl = new MissionControl(_mockRoverManager, _mockUserIo);
      _mockRoverManager.Stub(x => x.AllRoverStatus()).Return("Rover 0 is at 0, 0 facing N.");
      _mockRoverManager.Stub(x => x.RoverCount).Return(1);

      _missionControl.OutputRoverStatus();
      _mockRoverManager.AssertWasCalled(x => x.AllRoverStatus());
      _mockUserIo.AssertWasCalled(x => x.WriteLine("Rover 0 is at 0, 0 facing N."));
    }

    [Test]
    [ExpectedException(typeof(EmptyRoverListException))]
    public void OutputRoverStatusWhenNoRoversThrowsException() {
      _missionControl = new MissionControl(_mockRoverManager, null);
      _mockRoverManager.Stub(x => x.RoverCount).Return(0);

      _missionControl.OutputRoverStatus();
    }
  }
}
