using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public class Option : Model
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public override Model stringlify()
        {
            throw new NotImplementedException();
        }

        public override Model parse(string json)
        {
            throw new NotImplementedException();
        }
    }
}
