using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using SimpleBet.Services;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class BetController : ApiController
    {
        //Attributes
        private IDataService dataService = new DataService();

        //Constructors
        public BetController() { }

        // GET: api/values
        //[HttpGet]
        //public string Get()
        //{
        //    return JsonConvert.SerializeObject(this.dataService.GetBets());
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Bet bet = this.dataService.GetBet(id);
            if (bet == null)
            {
                return new HttpNotFoundResult();
            }
            else
            {
                string betJson = JsonConvert.SerializeObject(bet);
                return new ObjectResult(betJson);
            }
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Bet bet)
        {
            this.dataService.AddBet(bet);
            string betJson = JsonConvert.SerializeObject(bet);
            return new ObjectResult(betJson);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Bet bet)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != bet.Id)
            {
                return BadRequest();
            }

            try
            {
                this.dataService.UpdateBet(bet);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (dataService.GetBet(id) != null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //seriallize the object
            string json = JsonConvert.SerializeObject(bet);
            return Ok(json);
        }

        // DELETE api/values/5
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //}
    }
}