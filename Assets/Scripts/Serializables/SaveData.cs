using System.Collections.Generic;


[System.Serializable]
public class SaveEntry
{
    //public string playerName;
    public int score;
    public float time;
}

[System.Serializable]
public class LeaderboardData
{
    public List<SaveEntry> entries = new();
}