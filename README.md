# twitter-stats-api

Build using visual studio 2022

Target Framwork : .net 6.0

**EndPoints**
GET /api/sample-stream/start
  - starts the stream
  - response status codes
    - 200 : stream started
    - 204 : stream alread running
    - 503 : stream taking to long to start and canceled
    - 500 : unknown exception
GET /api/sample-stream/stop
  - stops the stream
  - response status codes
    - 200 : stream stopped
    - 500 : unknown exception
GET /api/tweets/count
  - returns the current count of tweets read from the stream
  - response status codes
    - 200 : stream stopped
    - 500 : unknown exception
GET /api/hashtags?numTags=10
  - returns the top hastags by count
  - query parameters
    - numTags : the number of hastags to return
  - response status codes
    - 200 : stream stopped
    - 500 : unknown exception
    
**Configuration**
configuration values (such as the api token) can be updated in the appsettings.{env}.json file found in the \src\TwitterStatistics folder

    
