using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CargoDispatcher.Tests
{
    [TestClass]
    public class DispatcherTests
    {
        [TestMethod]
        [DataRow("A", 5)]
        [DataRow("A,B", 5)]
        [DataRow("B,B", 5)]
        [DataRow("A,B,B", 7)]
        [DataRow("A,A,B,A,B,B,A,B", 29)]
        [DataRow("B,B,B,B,A,A,A,A", 49)]
        public void Dispatcher_Start_Should_Return(string given, int expected)
        {
            // Arrange
            var destinations = given.Split(',').Select(x => (Location)Enum.Parse(typeof(Location), x)).ToList().AsReadOnly();
            var dispatcher = new Dispatcher(destinations);

            // Act
            var time = dispatcher.Start();

            // Assert
            Assert.AreEqual(expected, time);
        }
    }
}
