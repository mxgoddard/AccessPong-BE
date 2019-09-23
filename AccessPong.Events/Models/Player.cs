using System;
using System.Collections.Generic;
using System.Text;

namespace AccessPong.Events.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public int Points { get; set; }
        public int Position { get; set; }
        public int MatchesPlayed { get; set; }
        public int MatchesWon { get; set; }
    }
}
