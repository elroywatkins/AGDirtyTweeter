namespace Tweeter
{
    public interface ITweeterReader
    {
        bool ReadFromSource();
    }
    public interface ITweeterInterpreter
    {
        bool ProcessTweets();
    }

    public interface ITweeterWriter
    {
        bool WriteToOutput();
    }
}
