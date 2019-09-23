using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using AccessPong.Events.Models;
using Json.Net;
using LiteDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccessPong.Events.Helper
{
    public class Helper : IHelper
    {
        private readonly ILogger<Helper> _logger;
        private readonly IConfiguration _configuration;

        private List<string> nameList;

        public Helper(ILogger<Helper> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        public void UpdateFixture(int fixtureId, int winnerId)
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    var col = db.GetCollection<Fixture>("tbl_fixtures");

                    var matchToUpdate = col.FindOne(Query.EQ("FixtureId", fixtureId));
                    matchToUpdate.WinnerId = winnerId;

                    col.Update(matchToUpdate);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: Failed to update fixture (fixtureId: {fixtureId}) with winnerId of {winnerId}. {ex.Message}");
                throw new Exception($"{DateTime.UtcNow}: Failed to update fixture (fixtureId: {fixtureId}) with winnerId of {winnerId}. {ex.Message}", ex);
            }
        }

        public string GetNextGame()
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    var col = db.GetCollection<Fixture>("tbl_fixtures");

                    var nextMatch = col.FindOne(Query.And(Query.EQ("WinnerId", -1), Query.All(1)));

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(nextMatch);

                    return jsonString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetFixtures()
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    // Read fixture table
                    var col = db.GetCollection<Fixture>("tbl_fixtures");

                    var data = col.Find(x => x.FixtureId > 0);

                    Fixtures fixtures = new Fixtures();
                    fixtures.fixtures = new List<Fixture>();

                    foreach (var item in data)
                    {
                        fixtures.fixtures.Add(item);
                    }

                    Console.WriteLine(fixtures);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fixtures);

                    return jsonString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return "";
            }
        }

        public string GetPlayers()
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    // Read fixture table
                    var col = db.GetCollection<Player>("tbl_players");

                    var data = col.Find(x => x.PlayerId > 0);

                    Players players = new Players();
                    players.players = new List<Player>();

                    foreach (var item in data)
                    {
                        players.players.Add(item);
                    }

                    Console.WriteLine(players);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(players);

                    return jsonString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return string.Empty;
            }
        }

        public bool GenerateFixtures()
        {
            _logger.LogInformation($"{DateTime.UtcNow}: Trying to generate fixtures.");

            try
            {
                // Get a list of names from .json file in data project
                nameList = new AccessPong.Data.Logic.Names().ConvertNames();

                // Create player list
                Players playerList = CreatePlayerList(nameList);
                if (playerList.players.Count == 0) return false;

                string dbFilePath = GetDatabasePathFromSettings();

                if (!PersistPlayers(playerList, dbFilePath)) return false;

                // Create fixtures
                Fixtures fixtureList = CreateFixtureList(playerList.players.Count);
                if (fixtureList.fixtures.Count == 0) return false;

                // Persist fixtures in database
                if (!PersistFixtures(fixtureList, dbFilePath)) return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return false;
            }

            return true;
        }

        // [!] This method will currently drop the whole tbl_fixtures table and re-create it
        public bool PersistFixtures(Fixtures fixtures, string databaseFilename)
        {
            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(databaseFilename))
                {
                    // Drop table
                    db.DropCollection("tbl_fixtures");

                    // Create fixtures table
                    var col = db.GetCollection<Fixture>("tbl_fixtures");

                    foreach (var fixture in fixtures.fixtures)
                    {
                        fixture.WinnerId = -1;
                        fixture.PlayerOneName = GetPlayerNameFromId(fixture.PlayerOneId, databaseFilename);
                        fixture.PlayerTwoName = GetPlayerNameFromId(fixture.PlayerTwoId, databaseFilename);
                        col.Insert(fixture);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return false;
            }
        }

        public bool PersistPlayers(Players players, string databaseFilename)
        {
            try
            { 
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(databaseFilename))
                {
                    // Drop table
                    db.DropCollection("tbl_players");

                    // Create fixtures table
                    var col = db.GetCollection<Player>("tbl_players");

                    foreach (var player in players.players)
                    {
                        col.Insert(player);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
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
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
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
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return new Players();
            }
        }

        public string GetDatabasePathFromSettings()
        {
            string sourceRoot = null;
            try
            {
                sourceRoot = this._configuration.GetSection("DatabaseFileLocations")["Test-Database"];
            }
            catch (Exception ex)
            {
                this._logger.LogError("A problem occurred getting the database path from configuration", ex);
            }

            if (string.IsNullOrEmpty(sourceRoot))
            {
                this._logger.LogError("No database path found in the configuration.");
            }

            return sourceRoot;
        }

        // This is a really bad and slow way to do things
        public string GetPlayerNameFromId(int playerId, string dbFilePath)
        {
            try
            {
                using (var db = new LiteDatabase(dbFilePath))
                {
                    // Fix this query
                    var col = db.GetCollection<Player>("tbl_players");

                    var allPlayers = col.Find(Query.All());

                    //var player = col.Find(x => x.PlayerId == playerId);
                    var player = allPlayers.First(x => x.PlayerId == playerId);

                    return player.PlayerName;
                    // return player.PlayerName;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return "";
            }
        }

        public string GetLeague()
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    // Read fixture table
                    var col = db.GetCollection<Player>("tbl_players");

                    var data = col.Find(x => x.PlayerId > 0);

                    Players players = new Players();
                    players.players = new List<Player>();

                    foreach (var item in data)
                    {
                        players.players.Add(item);
                    }

                    Console.WriteLine(players);

                    League league = new League();
                    league.league = new List<Player>();

                    league.league = players.players.OrderByDescending(o => o.Points).ToList();

                    // Calculate position off location in list.
                    var count = 1;
                    foreach (var player in league.league)
                    {
                        player.Position = count;
                        count++;
                    }

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(league);

                    return jsonString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return string.Empty;
            }
        }

        public bool FinishMatch(int fixtureId, int winnerId, int loserId)
        {
            try
            {
                // Update Fixture
                this.UpdateFixture(fixtureId, winnerId);

                // Update Player Stats
                this.UpdatePlayersAfterGame(winnerId, loserId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: {ex.Message}");
                return false;
            }
        }

        public void UpdatePlayersAfterGame(int winnerId, int loserId)
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    var col = db.GetCollection<Player>("tbl_players");

                    var allPlayers = col.Find(Query.All());

                    var winner = allPlayers.First(x => x.PlayerId == winnerId);
                    var loser = allPlayers.First(x => x.PlayerId == loserId);

                    winner.Points += 3;
                    winner.MatchesWon++;
                    winner.MatchesPlayed++;

                    loser.MatchesPlayed++;

                    col.Update(winner);
                    col.Update(loser);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: Failed to update winner, Id: {winnerId} and loser, Id: {loserId}", ex);
                throw new Exception($"{DateTime.UtcNow}: Failed to update winner, Id: {winnerId} and loser, Id: {loserId}", ex);
            }
        }

        public string GetFixture(int id)
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    var col = db.GetCollection<Fixture>("tbl_fixtures");

                    var allFixtures = col.Find(Query.All());
                    var fixture = allFixtures.First(x => x.FixtureId == id);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(fixture);

                    return jsonString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: Failed to find fixture by id: {id}", ex);
                throw new Exception($"{DateTime.UtcNow}: Failed to find fixture by id: {id}", ex);
            }
        }

        public string GetPlayer(int id)
        {
            string dbFilePath = GetDatabasePathFromSettings();

            try
            {
                // Open database or create if doesn't exist
                using (var db = new LiteDatabase(dbFilePath))
                {
                    var col = db.GetCollection<Player>("tbl_players");

                    var allPlayers = col.Find(Query.All());
                    var player = allPlayers.First(x => x.PlayerId == id);

                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(player);

                    return jsonString;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{DateTime.UtcNow}: Failed to find player by id: {id}", ex);
                throw new Exception($"{DateTime.UtcNow}: Failed to find player by id: {id}", ex);
            }
        }
    }
}
