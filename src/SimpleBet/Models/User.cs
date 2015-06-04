using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleBet.Models;

namespace SimpleBet.Models
{
    public class User : Model
    {
        public User()
        {
            this.Bets = new List<Bet>();
        }

        public int Id { get; set; }
        public Nullable<int> FacebookId { get; set; }

        public string Name { get; set; }

        public ICollection<Bet> Bets { get; set; }
    }
}
