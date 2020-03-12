using System;
using MarsRover;
using MarsRover.Interfaces;
using MarsRover.RoverTypes;
using NUnit.Framework;

namespace MarsRoverTests {
  [TestFixture]
  public class RoverFactoryTests {
    private IRoverFactory _iRoverFactory;

    [SetUp]
    public void SetUp() {
      _iRoverFactory = new RoverFactory();
    }

    [Test]
    public void CreateRoverWithTypeLandRoverCreatesLandRover() {
      var rover = _iRoverFactory.CreateRover("LandRover");
      Assert.IsInstanceOf<LandRover>(rover);
    }

    [Test]
    [ExpectedException(typeof(ApplicationException))]
    public void CreateRoverOfUnknownTypeThrowsException() {
      _iRoverFactory.CreateRover("UnknownRover");
    }
  }
}
