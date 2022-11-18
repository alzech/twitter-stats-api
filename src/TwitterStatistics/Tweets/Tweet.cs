namespace TwitterStatistics.Tweets
{
    public class Tweet
    {
        public Tweet(string id, List<string> tags)
        {
            Id = id;
            Hashtags = tags;
        }
        public string Id { get; set; }
        public List<string> Hashtags { get; set; }
    }
}
