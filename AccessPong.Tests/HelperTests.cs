using System;
using System.Collections.Generic;
using System.Text;
using AccessPong.Events.Helper;
using AccessPong.Events.Models;
using FluentAssertions;
using NUnit.Framework;

namespace AccessPong.Tests
{
    [TestFixture]
    public class HelperTests
    {
        private Helper _helper;

        [SetUp]
        public void Init()
        {
            _helper = new Helper();
        }

        [Test]
        public void GenerateFixtures_WholeMethod()
        {
            // Arrange
            bool expected = true;

            // Act
            bool actual = _helper.GenerateFixtures();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateFixtures_ThreePlayers()
        {
            // Arrange
            Fixtures expected = new Fixtures()
            {
                fixtures = new List<Fixture>()
                {
                    new Fixture()
                    {
                        FixtureId = 1,
                        PlayerOneId = 2,
                        PlayerTwoId = 3,
                    },
                    new Fixture()
                    {
                        FixtureId = 2,
                        PlayerOneId = 1,
                        PlayerTwoId = 3,
                    },
                    new Fixture()
                    {
                        FixtureId = 3,
                        PlayerOneId = 1,
                        PlayerTwoId = 2,
                    }
                }
            };

            // Act
            Fixtures actual = _helper.CreateFixtureList(3);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void CreateFixtures_FivePlayers()
        {
            // Arrange
            Fixtures expected = new Fixtures()
            {
                fixtures = new List<Fixture>()
                {
                    new Fixture()
                    {
                        FixtureId = 1,
                        PlayerOneId = 2,
                        PlayerTwoId = 5,
                    },
                    new Fixture()
                    {
                        FixtureId = 2,
                        PlayerOneId = 3,
                        PlayerTwoId = 4,
                    },
                    new Fixture()
                    {
                        FixtureId = 3,
                        PlayerOneId = 1,
                        PlayerTwoId = 5,
                    },
                    new Fixture()
                    {
                        FixtureId = 4,
                        PlayerOneId = 2,
                        PlayerTwoId = 3,
                    },
                    new Fixture()
                    {
                        FixtureId = 5,
                        PlayerOneId = 1,
                        PlayerTwoId = 4,
                    },
                    new Fixture()
                    {
                        FixtureId = 6,
                        PlayerOneId = 5,
                        PlayerTwoId = 3,
                    },
                    new Fixture()
                    {
                        FixtureId = 7,
                        PlayerOneId = 1,
                        PlayerTwoId = 3,
                    },
                    new Fixture()
                    {
                        FixtureId = 8,
                        PlayerOneId = 4,
                        PlayerTwoId = 2,
                    },
                    new Fixture()
                    {
                        FixtureId = 9,
                        PlayerOneId = 1,
                        PlayerTwoId = 2,
                    },
                    new Fixture()
                    {
                        FixtureId = 10,
                        PlayerOneId = 4,
                        PlayerTwoId = 5,
                    },
                }
            };

            // Act
            Fixtures actual = _helper.CreateFixtureList(5);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
