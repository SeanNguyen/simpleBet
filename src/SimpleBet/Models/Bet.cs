using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public class Bet : Model
    {
        public Bet()
        {
            this.Options = new List<Option>();
        }

        public int Id { get; set; }
        public string Question { get; set; }

        public ICollection<Option> Options { get; set; }

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
