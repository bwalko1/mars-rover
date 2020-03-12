using MarsRover;
using MarsRover.Interfaces;
using NUnit.Framework;

namespace MarsRoverTests {
  [TestFixture]
  public class RoverManagerTests {
    private IRoverFactory _roverFactory;
    private IRoverManager _roverManager;

    [SetUp]
    public void SetUp() {
      _roverFactory = new RoverFactory();
      _roverManager = new RoverManager(_roverFactory);
      _roverManager.CreateRover(0, 0, 'N');
    }

    [Test]
    public void CreateRoverAddsNewRoverToRoverList() {
      Assert.IsInstanceOf<IRover>(_roverManager.CurrentRover());
    }

    [Test]
    public void CreateMultipleRoversHasMostRecentlyCreatedRoverSelected() {
      _roverManager.CreateRover(1, 1, 'W');

      const string expected = "Rover 1 is at 1, 1 facing W.";
      Assert.AreEqual(expected, _roverManager.CurrentRover().GetStatus());
    }

    [Test]
    public void AllRoverStatusOutputsAllRoversInOrderWithCorrectValues() {
      _roverManager.CreateRover(1, 2, 'E');

      const string expected = "Rover 0 is at 0, 0 facing N.\n" +
                              "Rover 1 is at 1, 2 facing E.\n";
      Assert.AreEqual(expected, _roverManager.AllRoverStatus());
    }

    [Test]
    [TestCase(new[] { 0, 0 }, true)]
    [TestCase(new[] { 0, 1 }, false)]
    public void CheckCoordsReturnsTrueWhenSpaceIsOccupiedFalseWhenNotOccupied(int[] coords, bool expected) {
      Assert.AreEqual(expected, _roverManager.CheckCoords(coords));
    }
  }
}
