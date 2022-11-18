using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using Tweetinvi.Events.V2;
using TwitterStatistics.TwitterSampleStream;

namespace TwitterStatistics.Tweets
{
    public class TweetService : ITweetService
    {
        private ITweetRepo _twRepo;
        private ILogger<TweetService> _log;

        public TweetService(ILogger<TweetService> logger, ITweetRepo twRepo)
        {
            _twRepo = twRepo;
            _log = logger;
        }

        /// <summary>
        /// Event handler for tweets received
        /// </summary>
        /// <param name="sender">the sender of the even</param>
        /// <param name="args">the event arguments</param>
        public void TweetCountHandler(object? sender, TweetV2ReceivedEventArgs args)
        {
            var id = args.Tweet?.Id;
            if (id == null)
            {
                _log.LogInformation("Tweet received but no id. Tweet Not Saved.");
                return;
            }              
            var hasttagList = args.Tweet?.Entities?.Hashtags;
            if (hasttagList == null)
            {
                _log.LogInformation("Tweet received but no id. Tweet Not Saved.");
                return;
            }
            var tags = hasttagList.Select(x => x.Tag);
            var tweet = new Tweet(id, tags.Where(x => !string.IsNullOrEmpty(x)).ToList());
            _twRepo.PersistTweet(tweet);
        }

        /// <summary>
        /// Returns the current count of tweets in storage
        /// </summary>
        public int GetTweetCount() => _twRepo.GetTweetCount();
    }
}
