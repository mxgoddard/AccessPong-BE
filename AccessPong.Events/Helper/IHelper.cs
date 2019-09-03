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

        string GetFixtures();

        string GetDatabasePathFromSettings();

        string GetNextGame();
    }
}
