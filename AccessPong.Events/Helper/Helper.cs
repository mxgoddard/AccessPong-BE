using System;
using System.Collections.Generic;
using System.Text;
using AccessPong.Events.Models;

namespace AccessPong.Events.Helper
{
    public class Helper : IHelper
    {
        private List<string> nameList;

        public bool GenerateFixtures()
        {
            /*
             Purpose of this method is to create all the fixtures and persist in the database
             Will probably need to inject stuff for test mocking
             Return true on success, false on fail
             */

            try
            {
                // Get a list of names from .json file in data project
                nameList = new AccessPong.Data.Logic.Names().ConvertNames();

                // Create player list
                Players playerList = CreatePlayerList(nameList);
                if (playerList.players.Count == 0) return false;

                // Create fixtures
                Fixtures fixtureList = CreateFixtureList(playerList.players.Count);
                if (fixtureList.fixtures.Count == 0) return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // Log error
                return false;
            }

            return true;
        }

        public Fixtures CreateFixtureList(int playerCount)
        {
            try
            {
                var circularArray = new List<int>();

                for (int i = 0; i < playerCount; i++)
                {
                    circularArray.Add(i + 1);
                }

                // If odd amount of players add a dud player for sorting
                if (playerCount % 2 != 0)
                {
                    circularArray.Add(-1);
                }

                var fixtures = new Fixtures();
                fixtures.fixtures = new List<Fixture>();

                int gameCount = 0; // Redundant, just for testing
                int backPtr;
                int frontPtr;

                for (int i = 0; i < circularArray.Count - 1; i++)
                {
                    for (int j = 0; j < circularArray.Count / 2; j++)
                    {
                        backPtr = 0 + j;
                        frontPtr = circularArray.Count - j - 1;

                        if (circularArray[backPtr] != -1 && circularArray[frontPtr] != -1)
                        {
                            gameCount++;

                            var newFixture = new Fixture()
                            {
                                FixtureId = gameCount,
                                PlayerOneId = circularArray[backPtr],
                                PlayerTwoId = circularArray[frontPtr],
                            };

                            fixtures.fixtures.Add(newFixture);
                        }
                    }

                    // Loop items through circular queue
                    var copyList = new List<int>();
                    copyList.Add(circularArray[0]);
                    copyList.Add(circularArray[circularArray.Count - 1]);
                    for (int k = 2; k < circularArray.Count; k++)
                    {
                        copyList.Add(circularArray[k - 1]);
                    }

                    circularArray = copyList;
                }

                return fixtures;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Fixtures();
            }
        }

        public Players CreatePlayerList(List<string> nameList)
        {
            try
            {


                Players playerList = new Players();
                playerList.players = new List<Player>();

                for (var i = 0; i < nameList.Count; i++)
                {
                    var player = new Player()
                    {
                        PlayerId = i + 1,
                        PlayerName = nameList[i],
                    };

                    playerList.players.Add(player);
                }

                return playerList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Players();
            }
        }
    }
}
