using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public enum BETUSER_STATE
    {
        NONE,
        PENDING,
        CONFIRMED,
        DECLINED,
        VOTED,
        AGREE
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
        public virtual Bet Bet { get; set; }

        [Key, Column(Order = 1)]
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        
        public BETUSER_STATE State { get; set; }
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
