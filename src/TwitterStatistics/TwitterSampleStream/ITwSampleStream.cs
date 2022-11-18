using TwitterStatistics.Constants;

namespace TwitterStatistics.TwitterSampleStream
{
    public interface ITwSampleStream
    {
        Task<StreamStatus> Start();
        void Stop();
    }
}