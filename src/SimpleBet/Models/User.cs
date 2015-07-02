using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBet.Models
{
    public class User : Model
    {
        public User()
        {
            this.WinningItems = new List<WinningItem>();
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
        public ICollection<BetUser> Participations { get; set; } //removed virtural property here to disable lazy loading
        public virtual ICollection<WinningItem> WinningItems { get; set; }
        
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
