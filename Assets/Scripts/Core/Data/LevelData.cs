[System.Serializable]
public struct LevelData
{
    public int diamondsCollected;
    public bool[] isCollected;

    public LevelData(int diamondsLength)
    {
        this.isCollected = new bool[diamondsLength];

        diamondsCollected = 0;
    }
}
