using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleBet.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBet.Models
{
    public class User : Model
    {
        public User()
        {
            //this.Bets = new List<Bet>();
            this.Participations = new List<BetUser>();
        }

        //id
        public int Id { get; set; }
        [Index("FacebookId", 1, IsUnique = true)]
        public long FacebookId { get; set; }

        //presentation
        public string Name { get; set; }
        public string AvatarUrl { get; set; }

        //bets
        //public virtual ICollection<Bet> Bets { get; set; }
        public virtual ICollection<BetUser> Participations { get; set; }
        
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
