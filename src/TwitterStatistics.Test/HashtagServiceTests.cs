using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Collections.Concurrent;
using System.Dynamic;
using TwitterStatistics.Hashtags;
using TwitterStatistics.Tweets;

namespace TwitterStatistics.Test
{
    [Collection("Sequential")]
    public class HashtagServiceTests
    {
        [Fact]
        public void GetTopByCountTest()
        {
            //assign
            var twReopMock = new Mock<ITweetRepo>();
            twReopMock.Setup(x => x.GetTweets())
                .Returns(new ConcurrentBag<Tweet>() {
                    new Tweet("1", new List<string>() { "tag1", "tag2", "tag3", "tag4", "tag5", "tag6" }),
                    new Tweet("1", new List<string>() { "tag2", "tag3", "tag4", "tag5", "tag6" }),
                    new Tweet("1", new List<string>() { "tag3", "tag4", "tag5", "tag6"  }),
                    new Tweet("1", new List<string>() { "tag4", "tag5", "tag6"  }),
                    new Tweet("1", new List<string>() { "tag5", "tag6",  })
                });
            var sut = new HashtagService(twReopMock.Object);

            //act
            var result = sut.GetTopByCount(3);
            
            //assert
            Assert.Equal(3, result.Count); ;
            Assert.Equal("tag5", result[0].GetType().GetProperty("Tag").GetValue(result[0], null));
            Assert.Equal(5, result[0].GetType().GetProperty("Count").GetValue(result[0], null));
            Assert.Equal(5, result[1].GetType().GetProperty("Count").GetValue(result[1], null));
            Assert.Equal(4, result[2].GetType().GetProperty("Count").GetValue(result[2], null));

        }
    }
}