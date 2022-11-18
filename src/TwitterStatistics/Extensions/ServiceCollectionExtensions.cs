using Tweetinvi.Core.Models.Properties;
using TwitterStatistics.Hashtags;
using TwitterStatistics.Tweets;
using TwitterStatistics.TwitterApiClient;
using TwitterStatistics.TwitterSampleStream;

namespace TwitterStatistics.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Manually binding the configuration to the POCO, rather than having the options framework do it.
        /// Class properties matched to configuration keys my matching names  
        /// </summary>
        /// <typeparam name="TConfig">model class with properties to bind configuration values to</typeparam>
        /// <param name="services">application DI service collection</param>
        /// <param name="configuration">application configuration opject</param>
        /// <returns>object containing confiuration values</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TConfig ConfigurePOCO<TConfig>(this IServiceCollection services, IConfiguration configuration) where TConfig : class, new()
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configuration);

            var config = new TConfig();
            configuration.Bind(config);
            services.AddSingleton(config);
            return config;
        }

        /// <summary>
        /// Registers classes to the DI container
        /// </summary>
        /// <param name="services">application DI service collection</param>
        public static void ConfigureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddTransient<ITweetService, TweetService>();
            services.AddSingleton<ITwSampleStream, TwSampleStream>();
            services.AddTransient<IAppClient, AppClient>();
            services.AddTransient<ITweetRepo, TweetRepo>();
            services.AddTransient<IHashtagService, HashtagService>();
            services.AddMemoryCache();
            services.ConfigurePOCO<ClientAuthSettings>(config.GetSection("ClientAuthSettings"));

        }
    }
}
