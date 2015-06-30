using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public enum WINNING_ITEM_TYPE
    {
        MONETARY,
        NONMONETARY
    }

    public class WinningItem : Model
    {
        public int Id { get; set; }
        public WINNING_ITEM_TYPE Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }
        
        //public methods
        public override Model parse(dynamic data)
        {
            throw new NotImplementedException();
        }

        public override Model stringlify()
        {
            throw new NotImplementedException();
        }
    }
}
