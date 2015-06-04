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

        public string Id { get; set; }
        public string Question { get; set; }

        public ICollection<Option> Options { get; set; }
    }
}
