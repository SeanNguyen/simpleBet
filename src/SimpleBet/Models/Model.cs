using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Models
{
    public abstract class Model
    {
        public abstract Model stringlify();
        public abstract Model parse(String json);
    }
}
