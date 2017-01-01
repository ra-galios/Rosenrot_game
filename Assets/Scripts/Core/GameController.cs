using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : CreateSingletonGameObject<GameController>
{

    private int currentLevel;

    public LevelData[] levelsData;
    public static bool inGame;

    // Use this for initialization
    void OnEnable()
    {
        if (Market.Instance != null)     //если маркет загружен
        {
            DataManager.Instance.LoadGameData();
        }
    }

    public void LoadScene(string name)
    {
        DataManager.Instance.SaveGameData();

        SceneManager.LoadScene(name);
    }

    public void LoadActiveScene()
    {
        DataManager.Instance.SaveGameData();

        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
    }

    public void StopGame()
    {
        SetLevelDiamonds();
        LoadActiveScene();
    }

    private void SetLevelDiamonds()
    {
        if(Market.Instance.LocalDiamond > levelsData[CurrentLevel].diamondsCollected)
        {
            levelsData[CurrentLevel].diamondsCollected = Market.Instance.LocalDiamond;
        }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }
}
