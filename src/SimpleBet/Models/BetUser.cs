using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public enum BetUserState
    {
        NONE,
        PENDING,
        CONFIRMED,
        VOTED,
        DECLINED
    };

    public enum VoteCancelBetState
    {
        NONE,
        CREATOR,
        DISAGREE,
        AGREE,
    };

    public class BetUser : Model
    {
        //Attributes
        //Keys
        [Key, Column(Order = 0)]
        public int BetId { get; set; }
        [ForeignKey("BetId")]
        public Bet Bet { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        public BetUserState State { get; set; }
        public string Option { get; set; }


        //Cancelling action
        public VoteCancelBetState VoteCancelBetState { get; set; }

        //Public Methods
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
