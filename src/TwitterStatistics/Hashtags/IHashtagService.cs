namespace TwitterStatistics.Hashtags
{
    /// <summary>
    /// Calculates tweet count per hashtags
    /// </summary>
    /// <param name="num">number of hashtags to return</param>
    /// <returns>Hashtags with highest tweet count</returns>
    public interface IHashtagService
    {
        List<dynamic> GetTopByCount(int num);
    }
}