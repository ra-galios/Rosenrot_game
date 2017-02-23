
[System.Serializable]
public class GameData
{

    public LevelData[] LevelsData;

    public int health;
    public int seeds;
    public int bombs;
    public int diamonds;
    public int rubies;

    public string Log;

    public int curentlyAddHealth;

    public int[] tutorialsDisplays;

    public int[] achievements;

    public int[] Achievements
    {
        get
        {
            if (achievements == null)
            {
                achievements = new int[0];
            }
            return achievements;
        }
        set { achievements = value; }
    }

    public int[] TutorialsDisplays
    {
        get
        {
            if (tutorialsDisplays == null)
            {
                tutorialsDisplays = new int[0];
            }
            return tutorialsDisplays;
        }
        set { tutorialsDisplays = value; }
    }
}
