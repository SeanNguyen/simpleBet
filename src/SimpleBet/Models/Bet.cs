using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleBet.Models
{
    public enum BET_STATE
    {
        //init state
        NONE,
        //wait for everyone to accept or decline the bet
        PENDING,
        //everyone accepted or declined the bet
        CONFIRM,
        //someone choose to cancel the bet
        CANCELLING,
        CANCELLED,
        //everyone pick their options
        FINALLIZABLE,
        //someone choose to finallize the bet and now anyone can choose to dispute
        DISPUTABLE,
        //after everyone key in the correct option, the bet now officially closed
        FINALLIZED,
    }

    public enum BET_TYPE
    {
        MONETARY,
        NONMONETARY
    }

    public class Bet : Model
    {
        public Bet()
        {
            this.Options = new List<Option>();
            this.Participations = new List<BetUser>();
            //this.Participants = new List<User>();
        }

        //id
        public int Id { get; set; }

        //basic info
        [Required]
        public BET_TYPE BetType { get; set; }
        [Required, MaxLength(100)]
        public string Question { get; set; }
        public string WinningOption { get; set; }
        public virtual ICollection<Option> Options { get; set; }

        //winning item
        public int WinningItemId { get; set; }
        [ForeignKey("WinningItemId")]
        public WinningItem WinningItem { get; set; }

        //time
        [Required]
        public DateTime CreationTime { get; set; }
        [Required]
        public int Duration { get; set; } //this is in minute
        public DateTime? DisputeTime { get; set; }

        //user
        [Required]
        public int CreatorId { get; set; }
        public virtual ICollection<BetUser> Participations { get; set; }

        //state
        public BET_STATE State { get; set; }

        /*************************************************************/
        //public methods
        public override Model stringlify()
        {
            throw new NotImplementedException();
        }

        public override Model parse(dynamic data)
        {
            //if (data.question.Value != null)
            //{
            //    this.Question = data.question.Value;
            //}
            //if (data.creatorId.Value != null)
            //{
            //    this.CreatorId = data.creatorId.Value;
            //}
            //if (data.participants.HasValues)
            //{
            //    dynamic participants = data.participants;
            //    foreach(dynamic participant in participants)
            //    {
            //        string name = participant.name.Value;
            //        this.Participants.Add(name);
            //    }
            //}
            return this;
        }
    }
}
