using Newtonsoft.Json.Linq;
using System.Data.SqlTypes;
using System.Dynamic;
using Tweetinvi;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Events;
using Tweetinvi.Events.V2;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.V2;
using TwitterStatistics.Constants;
using TwitterStatistics.Tweets;
using TwitterStatistics.TwitterApiClient;

namespace TwitterStatistics.TwitterSampleStream
{
    public class TwSampleStream : ITwSampleStream
    {
        private TwitterClient _client;
        private ITweetService _tweetService;
        private ILogger<TwSampleStream> _log;
        private int _maxDelays;
        private StreamStatus _streamStatus;

        public ISampleStreamV2 TweetStream { get; }

        public TwSampleStream(ILogger<TwSampleStream> logger,  IConfiguration config, ITweetService tw, IAppClient appClient)
        {

            _client = appClient.TwClient;
            _tweetService = tw;
            TweetStream = Create(tw);
            _log = logger;
            _maxDelays = config.GetValue<int>("maxWaitIntervals");
            _streamStatus = StreamStatus.Stopped;
        }


        private ISampleStreamV2 Create(ITweetService tw)
        {
            var stream = _client.StreamsV2.CreateSampleStream();
            stream.TweetReceived += tw.TweetCountHandler;
            return stream;
        }

        /// <summary>
        /// Starts the sample stream
        /// </summary>
        /// <returns></returns>
        public async Task<StreamStatus> Start()
        {
            if (_streamStatus == StreamStatus.Started || _streamStatus == StreamStatus.Running)
            {
                _log.LogInformation($"Sample stream already starting");
                _streamStatus = StreamStatus.Running;
                return StreamStatus.Running;
            }
                
            try
            {
                var task = new Task(() => TweetStream.StartAsync());
                task.Start();
                _log.LogInformation($"Sample stream starting at {DateTime.Now}");
                var cnt = _tweetService.GetTweetCount();
                var checks = 1;
                while (cnt == 0 && checks < _maxDelays+1)
                {
                    await Task.Delay(1000);
                    cnt = _tweetService.GetTweetCount();
                    checks++;
                }
                if (checks >= _maxDelays)
                {
                    _log.LogInformation($"Sample stream taking too long to start. Max wait time {_maxDelays} second");
                    TweetStream.StopStream();
                    _streamStatus=StreamStatus.Canceled;
                    return StreamStatus.Canceled;
                }
                _log.LogInformation($"Sample stream started at {DateTime.Now}");
                _streamStatus = StreamStatus.Started;
                return StreamStatus.Started;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Sample stream unable to start.");
                TweetStream.StopStream();
                _streamStatus = StreamStatus.Failed;
                return StreamStatus.Failed;
            }


        }

        /// <summary>
        /// Stops a sample streem
        /// </summary>
        public void Stop()
        {
            try
            {
                TweetStream.StopStream();
                _streamStatus = StreamStatus.Stopped;
                _log.LogInformation($"Sample stream stopped at {DateTime.Now}");
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Sample stream unable to stop.");
                throw;
            }

        }
    }
}
