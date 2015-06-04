using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public class Option : Model
    {
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
