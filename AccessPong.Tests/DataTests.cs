using System;
using System.Collections.Generic;
using System.Text;
using AccessPong.Data.Logic;
using NUnit.Framework;

namespace AccessPong.Tests
{
    [TestFixture]
    public class DataTests
    {
        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void ReadJson()
        {
            // Arrange

            // 'expected' must match names.json file
            List<string> expected = new List<string>() { "Max", "Stefano", "Dave", "John" };

            AccessPong.Data.Logic.Names nameClass = new Names();

            // Act
            var actual = nameClass.ConvertNames();

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
