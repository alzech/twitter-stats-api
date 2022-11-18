using Tweetinvi.Events;
using Tweetinvi.Events.V2;

namespace TwitterStatistics.Tweets
{
    public interface ITweetService
    {
        /// <summary>
        /// Event handler for tweets received
        /// </summary>
        /// <param name="sender">the sender of the even</param>
        /// <param name="args">the event arguments</param>
        int GetTweetCount();

        /// <summary>
        /// Returns the current count of tweets in storage
        /// </summary>
        void TweetCountHandler(object? sender, TweetV2ReceivedEventArgs args);
    }
}