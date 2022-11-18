using Microsoft.AspNetCore.Mvc;
using TwitterStatistics.Constants;
using TwitterStatistics.TwitterSampleStream;

namespace TwitterStatistics.Stream
{
    [Route("api/sample-stream")]
    [ApiController]
    public class TwSampleStreamController : ControllerBase
    {
        private ITwSampleStream _twSTream;
        public TwSampleStreamController(ILogger<TwSampleStreamController> logger, ITwSampleStream twStream) 
        {
            _twSTream = twStream;
        }

        [HttpPut("start")]
        public async Task<IActionResult> StartStream()
        {
            var started = await _twSTream.Start();
            switch (started)
            {
                case StreamStatus.Started : 
                    return Ok("started");
                case StreamStatus.Running : 
                    return StatusCode(204, "already started");
                case StreamStatus.Canceled :
                    return StatusCode(503, "stream taking too long to start, canceled");
                case StreamStatus.Failed :
                    return StatusCode(500, "unknown exception");
                
            }


            return StatusCode(503, "stream did not start in time");
        }

        [HttpPut("stop")]
        public IActionResult StopStream()
        {
            _twSTream.Stop();
            return Ok("stop");
        }

    }
}
