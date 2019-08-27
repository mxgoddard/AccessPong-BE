using System;
using System.Collections.Generic;
using System.Text;

namespace AccessPong.Events.Models
{
    public class Fixture
    {
        public int Id { get; set; }
        public int FixtureId { get; set; }
        public int PlayerOneId { get; set; }
        public int PlayerTwoId { get; set; }
    }
}
