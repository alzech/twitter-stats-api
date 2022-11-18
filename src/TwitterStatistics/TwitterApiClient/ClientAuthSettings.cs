namespace TwitterStatistics.TwitterApiClient
{
    public class ClientAuthSettings
    {
        public string ApiKey { get; set; }
        public string ApiKeySecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
        public string BrearerToken { get; set; }

        public ClientAuthSettings()
        {
            ApiKey = "";
            ApiKeySecret = "";
            AccessToken = "";
            AccessTokenSecret = "";
            BrearerToken = "";
        }

    }
}
