using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace TwitterStatistics.Tweets
{
    public class TweetRepo : ITweetRepo
    {
        private IMemoryCache _memoryCache;
        private MemoryCacheEntryOptions _cacheOptions;

        public TweetRepo(IMemoryCache memCache)
        {
            _memoryCache = memCache;
            _cacheOptions = new MemoryCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromHours(36)
            };           
        }

        /// <summary>
        /// Adds a tweet to storage to be retreived latter
        /// </summary>
        /// <param name="tw">The tweet to add</param>
        public void PersistTweet(Tweet tw)
        {
            if (_memoryCache.TryGetValue<ConcurrentBag<Tweet>>("Tweets", out ConcurrentBag<Tweet> tweets))
            {
                tweets.Add(tw);
            }
            else
            {
                var bag = new ConcurrentBag<Tweet>();
                bag.Add(tw);
                _memoryCache.Set<ConcurrentBag<Tweet>>("Tweets", bag, _cacheOptions);
            }
                
        }

        public ConcurrentBag<Tweet> GetTweets()
        {
            if (_memoryCache.TryGetValue<ConcurrentBag<Tweet>>("Tweets", out ConcurrentBag<Tweet> tweets))
            {
                return tweets;
            }
            return new ConcurrentBag<Tweet> { };

        }

        public int GetTweetCount()
        {
            if (_memoryCache.TryGetValue<ConcurrentBag<Tweet>>("Tweets", out ConcurrentBag<Tweet> tweets))
            {
                return tweets.Count;
            }
            return 0;

        }
    }
}
