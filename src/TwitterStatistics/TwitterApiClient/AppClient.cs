using Tweetinvi;

namespace TwitterStatistics.TwitterApiClient
{
    public class AppClient : IAppClient
    {
        public TwitterClient TwClient { get; }

        public AppClient(ClientAuthSettings authSettings)
        {
            TwClient = Create(authSettings.AccessToken, authSettings.AccessTokenSecret, authSettings.BrearerToken);
        }

        private TwitterClient Create(string accessToken, string accessTokenSecret, string bearerToken)
        {
            return new TwitterClient(accessToken, accessTokenSecret, bearerToken);
        }

    }
}
