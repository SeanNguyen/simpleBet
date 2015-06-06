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

        //id
        public int Id { get; set; }
        public long FacebookId { get; set; }

        //presentation
        public string Name { get; set; }
        public string AvataUrl { get; set; }

        //bets
        public ICollection<Bet> Bets { get; set; }
        
        /*************************************************************/
        //public methods
        public override Model stringlify()
        {
            throw new NotImplementedException();
        }

        public override Model parse(dynamic data)
        {
            if (data.name.Value != null)
            {
                this.Name = data.name.Value;
            }
            if (data.facebookId.Value != null)
            {
                this.FacebookId = data.facebookId.Value;
            }
            return this;
        }
    }
}
