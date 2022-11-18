using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Dynamic;

namespace TwitterStatistics.IntegrationTests
{
    [Collection("Sequential")]
    public class TweetIntegrationTests
    {
        [Fact]
        public async Task TotalNumberTweetsTest_NotStarted()
        {
            //arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            //act
            var response = await httpClient.GetAsync("api/tweets/count");

            //assert
            var stringResult = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<ExpandoObject>(stringResult);
            Assert.Equal(0, obj.GetProperty<long>("tweetCount"));
        }

        [Fact]
        public async Task TotalNumberTweetsTest_Started()
        {
            //arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            //act
            var startResp = await httpClient.GetAsync("api/sample-stream/start");
            await Task.Delay(1000);
            var firstResp = await httpClient.GetAsync("api/tweets/count");
            await Task.Delay(2000);
            var secondResp = await httpClient.GetAsync("api/tweets/count");
            var stopResp = await httpClient.GetAsync("api/sample-stream/stop");

            //assert
            var firstStringResp = await firstResp.Content.ReadAsStringAsync();
            var first = JsonConvert.DeserializeObject<ExpandoObject>(firstStringResp);
            var secondStringResp = await secondResp.Content.ReadAsStringAsync();
            var second = JsonConvert.DeserializeObject<ExpandoObject>(secondStringResp);
            Assert.NotEqual(first.GetProperty<long>("tweetCount"), second.GetProperty<long>("tweetCount"));
        }
    }
}
