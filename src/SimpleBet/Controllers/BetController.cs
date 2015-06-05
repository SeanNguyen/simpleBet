using Microsoft.AspNet.Mvc;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
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
            foreach(Bet bet in MockDb.Bets)
            {
                if(bet.Id == id)
                {
                    return new ObjectResult(bet);
                }
            }
            return new HttpNotFoundResult();
            //if (bet == null)
            //{
            //    return new HttpNotFoundResult();
            //}
            //else
            //{
            //    return new ObjectResult(bet);
            //}
        }

        // POST api/values
        [HttpPost]
        public string Post([FromBody]dynamic data)
        {
            Bet newBet = new Bet();
            newBet.parse(data);
            MockDb.Bets.Add(newBet);
            return newBet.Id.ToString();
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
