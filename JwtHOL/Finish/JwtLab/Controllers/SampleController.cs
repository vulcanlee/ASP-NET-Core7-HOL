using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// https://www.syncfusion.com/blogs/post/how-to-build-crud-rest-apis-with-asp-net-core-3-1-and-entity-framework-core-create-jwt-tokens-and-secure-apis.aspx

namespace JwtLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SampleController : ControllerBase
    {
        // GET: api/<SampleController>
        [HttpGet]
        public ActionResult<List<MyUser>> Get()
        {
            return MyUser.GetMyUsers();
        }

        // GET api/<SampleController>/5
        [HttpGet("{id}")]
        public ActionResult<MyUser> Get(int id)
        {
            var user = MyUser.GetMyUsers()
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST api/<SampleController>
        [HttpPost]
        public ActionResult<MyUser> Post([FromBody] MyUser user)
        {
            if (string.IsNullOrEmpty(user.Account)) return BadRequest();

            // Todo : Create this record

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        // PUT api/<SampleController>/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] MyUser user)
        {
            if(id!=user.Id) return BadRequest();
            if (string.IsNullOrEmpty(user.Account)) return BadRequest();

            // Todo : Update this record

            return NoContent();
        }

        // DELETE api/<SampleController>/5
        [HttpDelete("{id}")]
        public ActionResult<MyUser> Delete(int id)
        {
            var user = MyUser.GetMyUsers()
                .FirstOrDefault(x=>x.Id == id);
            if (user == null) return NotFound();

            // Todo : Delete this record

            return NoContent();
        }
    }
}
