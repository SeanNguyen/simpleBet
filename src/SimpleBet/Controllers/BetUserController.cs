using Microsoft.AspNet.Mvc;
using Newtonsoft.Json;
using SimpleBet.Data;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class BetUserController : Controller
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public BetUserController()
        {
            this.dbContext = new SimpleBetContext();
        }

        // GET: api/values
        [HttpGet]
        public string Get()
        {
            return JsonConvert.SerializeObject(this.dbContext.Users);
        }

        //currently cannot get by Id will add later if required
        // GET api/values/5
        //[HttpGet("{id:int}")]
        //public IActionResult Get(int id)
        //{
        //    BetUser user = this.dbContext.BetUsers.Find(id);
        //    if (user == null)
        //    {
        //        return new HttpNotFoundResult();
        //    }
        //    else
        //    {
        //        string userJson = JsonConvert.SerializeObject(user);
        //        return new ObjectResult(userJson);
        //    }
        //}
        
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]BetUser betUser)
        {
            this.dbContext.BetUsers.Add(betUser);
            this.dbContext.SaveChanges();
            string json = JsonConvert.SerializeObject(betUser);
            return new ObjectResult(json);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]BetUser betUser)
        {
            BetUser betUserSource = this.dbContext.BetUsers
                                            .Where(bu => bu.UserId == betUser.UserId)              
                                            .FirstOrDefault();
            betUserSource.State = betUser.State;
            this.dbContext.SaveChanges();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
