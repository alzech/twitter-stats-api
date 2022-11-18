using Tweetinvi;

namespace TwitterStatistics.TwitterApiClient
{
    public interface IAppClient
    {
        TwitterClient TwClient { get; }
    }
}