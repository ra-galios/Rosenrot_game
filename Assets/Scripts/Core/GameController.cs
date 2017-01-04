using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : CreateSingletonGameObject<GameController>
{

    private int currentLevel;
    private List<string> levelsPath = new List<string>();

    public LevelData[] levelsData;
    public int totalGameLevels;
    public bool inGame;

    // Use this for initialization
    void OnEnable()
    {
        GetData();
    }

    public void AddHealth()
    {
        Market.Instance.Health = 100 ;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadPrevScene();
        }
    }

    public void LoadScene(string name)
    {
        if (!levelsPath.Contains(SceneManager.GetActiveScene().name))
        {
            levelsPath.Add(SceneManager.GetActiveScene().name);
        }

        DataManager.Instance.SaveGameData();
        SceneManager.LoadScene(name);
    }

    public void LoadActiveScene()
    {
        DataManager.Instance.SaveGameData();

        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(name);
    }


    public void LoadPrevScene()
    {
        if (levelsPath.Count > 0)
        {
            string name = levelsPath[levelsPath.Count - 1];
            levelsPath.RemoveAt(levelsPath.Count - 1);

            DataManager.Instance.SaveGameData();
            SceneManager.LoadScene(name);
        }
    }

    public void StopGame()
    {
        SetLevelDiamonds();
        LoadActiveScene();
    }

    private void SetLevelDiamonds()
    {
        if (Market.Instance.LocalDiamond > levelsData[currentLevel].diamondsCollected)
        {
            levelsData[currentLevel].diamondsCollected = Market.Instance.LocalDiamond;
        }
    }

    void GetData()
    {
        if (Market.Instance != null)     //если маркет загружен
        {
            DataManager.Instance.LoadGameData();
        }
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }
}
