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

                // Create fixtures
                Fixtures fixtureList = CreateFixtureList(playerList);

                Console.WriteLine(playerList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);  // Log error
                return false;
            }

            return true;
        }

        public Fixtures CreateFixtureList(Players playerList)
        {
            // Perform actual logic here :)

            return new Fixtures();
        }

        public Players CreatePlayerList(List<string> nameList)
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
    }
}
