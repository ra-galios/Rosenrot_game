[System.Serializable]
public struct LevelData
{
    public int diamondsCollected;
    public bool[] isCollected;

    public LevelData(int diamondsLength)
    {
        isCollected = new bool[diamondsLength];

        diamondsCollected = 0;
    }

    public bool[] IsCollected
    {
        get
        {
            if(isCollected == null)
            {
                isCollected = new bool[0];
            }
            return isCollected;
        }
        set
        {
            isCollected = value;
        }
    }
}
