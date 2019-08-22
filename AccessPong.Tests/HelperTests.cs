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
        public void CreateFixtures()
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

            Players players = new Players()
            {
                players = new List<Player>()
                {
                    new Player()
                    {
                        PlayerId = 1,
                        PlayerName = "Max",
                    },
                    new Player()
                    {
                        PlayerId = 2,
                        PlayerName = "Stefano",
                    },
                    new Player()
                    {
                        PlayerId = 3,
                        PlayerName = "Dave",
                    }
                }
            };

            // Act
            Fixtures actual = _helper.CreateFixtureList(players);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
