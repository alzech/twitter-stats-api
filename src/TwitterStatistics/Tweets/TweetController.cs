using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TwitterStatistics.Tweets
{
    [Route("api/tweets")]
    [ApiController]
    public class TweetController : Controller
    {
        private ITweetService _tweet;
        public TweetController(ITweetService tweet)
        {
            _tweet = tweet;
        }

        [HttpGet("count")]
        public IActionResult TotalNumberTweets()
        {
            var cnts = _tweet.GetTweetCount();
            return Ok(new { TweetCount = cnts});
        }
    }
}
