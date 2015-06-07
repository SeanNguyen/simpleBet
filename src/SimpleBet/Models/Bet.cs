using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Required, MaxLength(100)]
        public string Question { get; set; }
        [Required]
        public virtual ICollection<Option> Options { get; set; }

        //time
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; }
        [Required]
        public int Duration { get; set; } //this is in minute

        //user
        [Required]
        public User Creator { get; set; }
        [Required, MaxLength(10)]
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
