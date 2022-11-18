using Microsoft.Extensions.Configuration;
using Serilog;
using TwitterStatistics.Exceptions;
using TwitterStatistics.Extensions;
using TwitterStatistics.Hashtags;
using TwitterStatistics.Tweets;
using TwitterStatistics.TwitterApiClient;
using TwitterStatistics.TwitterSampleStream;

var builder = WebApplication.CreateBuilder(args);

//load configuration file
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{env}.json", false, true)
    .Build();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.ConfigureServices(config);

//logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .Enrich.FromLogContext()
        .CreateLogger());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthorization();
app.MapControllers();
app.Run();
