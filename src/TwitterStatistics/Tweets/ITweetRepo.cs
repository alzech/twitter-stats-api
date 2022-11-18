using System.Collections.Concurrent;

namespace TwitterStatistics.Tweets
{
    public interface ITweetRepo
    {
        /// <summary>
        /// Adds a tweet to storage to be retreived latter
        /// </summary>
        /// <param name="tw">The tweet to add</param>
        void PersistTweet(Tweet tw);

        ConcurrentBag<Tweet> GetTweets();

        int GetTweetCount();
    }
}