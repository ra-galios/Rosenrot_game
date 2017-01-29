
[System.Serializable]
public class GameData
{

    public LevelData[] LevelsData;

    public int curentlyAddHealth;
    public int health;
    public int seeds;
    public int bombs;
    public int diamonds;
    public int rubies;

    public int[] achievements;

    public int[] Achievements
    {
        get
        {
			if(achievements == null)
			{
				achievements = new int[0];
			}
            return achievements;
        }
        set { achievements = value; }
    }
}
