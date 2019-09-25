using System;
using System.Collections.Generic;
using System.Text;
using AccessPong.Events.Models;

namespace AccessPong.Events.Helper
{
    public interface IHelper
    {
        bool GenerateFixtures();

        Players CreatePlayerList(List<string> nameList);

        Fixtures CreateFixtureList(int playerCount);

        bool PersistFixtures(Fixtures fixtures, string databaseFilename);

        bool PersistPlayers(Players players, string databaseFilename);

        string GetFixtures();

        string GetPlayers();

        string GetLeague();

        string GetDatabasePathFromSettings();

        string GetNextGame();

        void UpdateFixture(int fixtureId, int winnerId);

        string GetPlayerNameFromId(int playerId, string dbFilePath);

        bool FinishMatch(int fixtureId, int winnerId, int loserId);

        void UpdatePlayersAfterGame(int winnerId, int loserId);

        string GetFixture(int id);

        string GetPlayer(int id);

        string GetRules();
    }
}
