using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBet.Models
{
    public class Option : Model
    {
        [Key, Column(Order = 0)]
        public int BetId { get; set; }
        public Bet Bet { get; set; }
        [Key, Column(Order = 1), MaxLength(30)]
        public string Content { get; set; }

        public override Model stringlify()
        {
            throw new NotImplementedException();
        }

        public override Model parse(dynamic data)
        {
            throw new NotImplementedException();
        }
    }
}
