using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using TwitterStatistics.Tweets;

namespace TwitterStatistics.Hashtags
{
    public class HashtagService : IHashtagService
    {
        private ITweetRepo _twRepo;

        public HashtagService(ITweetRepo twRepo)
        {
            _twRepo = twRepo;
        }

        /// <summary>
        /// Calculates tweet count per hashtags
        /// </summary>
        /// <param name="num">number of hashtags to return</param>
        /// <returns>Hashtags with highest tweet count</returns>
        public List<dynamic> GetTopByCount(int num)
        {
            var tweets = _twRepo.GetTweets();
            if (tweets == null)
                return new List<object>() { };

            var hashtags = tweets.SelectMany(x => x.Hashtags);
            return hashtags.GroupBy(x => x)
                .Select(x => new { Tag = x.Key, Count = x.Count() })
                .OrderByDescending(x => x.Count).ThenBy(x => x.Tag)
                .Take(num)
                .ToList<dynamic>();
        }
    }
}
