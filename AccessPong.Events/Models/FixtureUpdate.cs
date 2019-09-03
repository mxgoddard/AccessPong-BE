using System;
using System.Collections.Generic;
using System.Text;

namespace AccessPong.Events.Models
{
    public class FixtureUpdate
    {
        public int FixtureId { get; set; }
        public int WinnerId { get; set; }
    }
}
