using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AccessPong.Events.Models;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace AccessPong.Events.Helper
{
    public class Helper : IHelper
    {
        private readonly ILogger<Helper> _logger;

        private List<string> nameList;
        private readonly string databaseFilename = "TEST-AccessPongDB"; // Move to config file

        public Helper(ILogger<Helper> logger)
        {
            this._logger = logger;
        }

        public bool GenerateFixtures()
        {
            /*
            This won't be connected to any endpoints, only used for generating fixtures

             Purpose of this method is to create all the fixtures and persist in the database
             Will probably need to inject stuff for test mocking
             Return true on success, false on fail
             */

            _logger.LogInformation($"{DateTime.UtcNow}: Trying to generate fixtures.");

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

                // Persist fixtures in database
                if (!PersistFixtures(fixtureList, databaseFilename)) return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return false;
            }

            return true;
        }

        // [!] This method will currently drop the whole tbl_fixtures table
        public bool PersistFixtures(Fixtures fixtures, string databaseFilename)
        {
            try
            {
                string path = Directory.GetCurrentDirectory();
                string dbFilePath = Path.GetFullPath(Path.Combine(path, $@"..\..\..\..\AccessPong.Data\Database\{databaseFilename}.db"));

                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    // Drop table
                    db.DropCollection("tbl_fixtures");

                    // Create fixtures table
                    var col = db.GetCollection<Fixture>("tbl_fixtures");

                    foreach (var fixture in fixtures.fixtures)
                    {
                        col.Insert(fixture);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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
