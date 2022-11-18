using Microsoft.Extensions.Logging;
using Moq;
using Tweetinvi.Events.V2;
using Tweetinvi.Models.V2;
using TwitterStatistics.Tweets;

namespace TwitterStatistics.Test
{
    [Collection("Sequential")]
    public class TweetServiceTests
    {
        [Fact]
        public void TweetCountHandlerTest()
        {
            //assign
            var entities = new TweetEntitiesV2();
            entities.Hashtags = new HashtagV2[] { new HashtagV2() { Tag = "hashtag1" }, new HashtagV2() { Tag = "hashtag2" } };  
            var tweet = new TweetV2();
            tweet.Id = "1";           
            tweet.Entities = entities;
            var resp = new TweetV2Response();
            resp.Tweet = tweet;
            var args = new TweetV2ReceivedEventArgs(resp, "");

            Tweet? returnedTweet = null;
            var twReopMock = new Mock<ITweetRepo>();
            twReopMock.Setup(x => x.PersistTweet(It.IsAny<Tweet>())).Callback<Tweet>(obj => returnedTweet = obj);

            var loggerMock = new Mock<ILogger<TweetService>>();

            var sut = new TweetService(loggerMock.Object, twReopMock.Object);

            //act
            sut.TweetCountHandler(null, args);

            //assert
            twReopMock.Verify(x => x.PersistTweet(It.IsAny<Tweet>()), Times.Once);
            Assert.Equal("1", returnedTweet?.Id);
            Assert.Contains<string>("hashtag1", returnedTweet?.Hashtags);
            Assert.Contains<string>("hashtag2", returnedTweet?.Hashtags);

        }

        [Fact]
        public void GetTweetCountTest()
        {
            //assign
            var twReopMock = new Mock<ITweetRepo>();
            twReopMock.Setup(x => x.GetTweetCount()).Returns(432);

            var loggerMock = new Mock<ILogger<TweetService>>();

            var sut = new TweetService(loggerMock.Object, twReopMock.Object);

            //act
            var results = sut.GetTweetCount();

            //assert
            twReopMock.Verify(x => x.GetTweetCount(), Times.Once);
            Assert.Equal(432, results);

        }
    }
}
