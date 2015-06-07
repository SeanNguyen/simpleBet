using Microsoft.AspNet.Mvc;
using SimpleBet.Data;
using SimpleBet.Models;
using System.Collections.Generic;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class BetController
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public BetController()
        {
            this.dbContext = new SimpleBetContext();
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
            return null;
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