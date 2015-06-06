using Newtonsoft.Json.Linq;
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
            this.Participants = new List<User>();
        }

        //id
        public int Id { get; set; }

        //basic info
        public string Question { get; set; }
        public virtual ICollection<Option> Options { get; set; }

        //time
        public DateTime CreationDate { get; set; }
        public int Duration { get; set; } //this is in minute

        //user
        public string CreatorId { get; set; }
        public virtual ICollection<User> Participants { get; set; }

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
