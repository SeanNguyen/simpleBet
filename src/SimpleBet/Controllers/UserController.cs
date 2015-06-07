using Microsoft.AspNet.Mvc;
using SimpleBet.Data;
using SimpleBet.Models;
using System.Collections.Generic;

namespace SimpleBet.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        //Attributes
        private readonly SimpleBetContext dbContext;

        //Constructors
        public UserController(SimpleBetContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return this.dbContext.Users;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return new HttpNotFoundResult();
            //User user = this.dbContext.Users.FirstOrDefault(v => v.Id == id);
            //if (user == null)
            //{
            //    return new HttpNotFoundResult();
            //}
            //else
            //{
            //    return new ObjectResult(user);
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