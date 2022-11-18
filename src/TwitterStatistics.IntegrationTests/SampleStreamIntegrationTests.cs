using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStatistics.IntegrationTests
{
    [Collection("Sequential")]
    public class SampleStreamIntegrationTests
    {
        [Fact]
        public async Task StartedTest()
        {
            //arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            //act
            var startResp = await httpClient.GetAsync("api/sample-stream/start");
            await Task.Delay(1000);
            var tweetResp = await httpClient.GetAsync("api/tweets/count");
            var stopResp = await httpClient.GetAsync("api/sample-stream/stop");

            //assert
            var tweetStringResp = await tweetResp.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<ExpandoObject>(tweetStringResp);
            Assert.NotEqual(0, obj.GetProperty<long>("tweetCount"));
        }

        [Fact]
        public async Task StoppedTest()
        {
            //arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            //act
            var startResp = await httpClient.GetAsync("api/sample-stream/start");
            await Task.Delay(1000);
            var tweetResp = await httpClient.GetAsync("api/tweets/count");
            await Task.Delay(1000);
            var stopResp = await httpClient.GetAsync("api/sample-stream/stop");
            var firstAfterStopResp = await httpClient.GetAsync("api/tweets/count");
            await Task.Delay(2000);
            var secondAfterStopResp = await httpClient.GetAsync("api/tweets/count");

            //assert
            var tweetStringResp = await tweetResp.Content.ReadAsStringAsync();
            var tweetobj = JsonConvert.DeserializeObject<ExpandoObject>(tweetStringResp);
            var firstAfterStringResp = await firstAfterStopResp.Content.ReadAsStringAsync();
            var first = JsonConvert.DeserializeObject<ExpandoObject>(firstAfterStringResp);
            var secondAfterStringResp = await secondAfterStopResp.Content.ReadAsStringAsync();
            var second = JsonConvert.DeserializeObject<ExpandoObject>(secondAfterStringResp);
            Assert.NotEqual(tweetobj.GetProperty<long>("tweetCount"), first.GetProperty<long>("tweetCount"));
            Assert.Equal(first.GetProperty<long>("tweetCount"), second.GetProperty<long>("tweetCount"));
        }
    }
}
