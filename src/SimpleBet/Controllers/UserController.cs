using Microsoft.AspNet.Mvc;
using SimpleBet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            foreach (User user in MockDb.Users)
            {
                if (user.Id == id)
                {
                    return new ObjectResult(user);
                }
            }
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
            User user = new User();
            user.parse(data);
            MockDb.Users.Add(user);
            return user.Id.ToString();
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
