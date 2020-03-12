using MarsRover.Interfaces;
using MarsRover.RoverTypes;
using NUnit.Framework;

namespace MarsRoverTests {

  [TestFixture]
  public class RoverTests {
    private IRover _defaultRover;

    [SetUp]
    public void SetUp() {
      _defaultRover = new LandRover();
      _defaultRover.SetRoverValues(0, 0, 0, 'N');
    }

    [Test]
    public void RoverHasProperValuesWhenCreated() {
      var rover = new LandRover(1, 2, 3, 'W');

      Assert.AreEqual(new[] { 1, 2 }, rover.GetCoords());
      Assert.AreEqual(3, rover.RoverId);
      Assert.AreEqual('W', rover.Direction);
    }

    [Test]
    [TestCase('N', 'E')]
    [TestCase('E', 'S')]
    [TestCase('S', 'W')]
    [TestCase('W', 'N')]
    public void RoverTurnsRightCorrectly(char startDirection, char expected) {
      var rover = new LandRover(0, 0, 0, startDirection);
      rover.TurnRight();

      Assert.AreEqual(expected, rover.Direction);
    }

    [Test]
    [TestCase('N', 'W')]
    [TestCase('W', 'S')]
    [TestCase('S', 'E')]
    [TestCase('E', 'N')]
    public void RoverTurnsLeftCorrectly(char startDirection, char expected) {
      var rover = new LandRover(0, 0, 0, startDirection);
      rover.TurnLeft();

      Assert.AreEqual(expected, rover.Direction);
    }

    [Test]
    public void RoverPrintsStatusCorrectly() {
      Assert.AreEqual("Rover 0 is at 0, 0 facing N.", _defaultRover.GetStatus());
    }

    [Test]
    public void RoverMovesForwardCorrectly() {
      _defaultRover.MoveForward();
      Assert.AreEqual(new[] { 0, 1 }, _defaultRover.GetCoords());
    }
  }
}
