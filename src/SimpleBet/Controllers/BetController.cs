using Microsoft.AspNet.Mvc;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Controllers
{
    public class BetController
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public BetController(SimpleBetContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<Bet> Get()
        {
            return this.dbContext.Bets;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Bet bet = this.dbContext.Bets.FirstOrDefault(v => v.Id == id);
            if (bet == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                return new ObjectResult(bet);
            }
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            Bet newBet = new Bet();
            newBet.parse(value);
            this.dbContext.Bets.Add(newBet);
            this.dbContext.SaveChanges();
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
