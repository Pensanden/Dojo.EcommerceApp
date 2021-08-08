using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private StoreContext _ctx;
        public BuggyController(StoreContext ctx)
        {
            _ctx = ctx;
        }

        //this controller is for testing error handeling do not use in production
        [HttpGet("NotFound")]
        public ActionResult GetNotFoundRequest()
        {
           var thing =  _ctx.Products.Find(42);
            if (thing == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok();
        }

        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var thing = _ctx.Products.Find(42);

            var thingToReturn = thing.ToString();
            
            return Ok();
        }

        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }
        
        [HttpGet("Badrequest/{id}")]
        public ActionResult GetNotFountRequest(int id)
        {
            return Ok();
        }
    }
}