using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TwitterStatistics.Hashtags
{
    [Route("api/hashtags")]
    [ApiController]
    public class HashtagController : ControllerBase
    {
        private IHashtagService _hashtagService;
        public HashtagController(IHashtagService hashtagService)
        {
            _hashtagService = hashtagService;
        }

        [HttpGet()]
        public IActionResult TopHashtags([FromQuery] int numTags)
        {
            if (numTags < 1)
            {
                ModelState.AddModelError("numTags", "Requested number of tag must be greater then 0.");
                return BadRequest(ModelState);
            }
            return Ok(_hashtagService.GetTopByCount(numTags));
        }
    }

}
