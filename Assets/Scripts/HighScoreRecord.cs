public class HighScoreRecord
{
    private int position;
    private string name;
    private int score;
    private int timeInSeconds;

    public HighScoreRecord(int positionValue, string nameValue, int scoreValue, int timeInSecondsValue)
    {
        position = positionValue;
        name = nameValue;
        score = scoreValue;
        timeInSeconds = timeInSecondsValue;
    }

    public string GetName()
    {
        return name;
    }

    public int GetPosition()
    {
        return position;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetTimeInSeconds()
    {
        return timeInSeconds;
    }

    public override string ToString()
    {
        return "HighScoreRecord: " + position + ", " + name + ", " + score + ", " + timeInSeconds;
    }
}