using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : CreateSingletonGameObject<GameController>
{

    private int currentLevel;
    //private List<string> levelsPath = new List<string>();

    public LevelData[] levelsData;
    public int totalGameLevels = 14;
    public bool inGame;
    public PlayerBehaviour playerBeh = null;

    // Use this for initialization
    void OnEnable()
    {
        GetData();
    }

    public void AddHealth()
    {
        Market.Instance.Health += 100;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadFirstScene();
        }
    }

    public void LoadScene(string name)
    {
        DataManager.Instance.SaveGameData();
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadLevel(name));
    }

    public void LoadActiveScene()
    {
        DataManager.Instance.SaveGameData();

        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadLevel(name));
    }


    public void LoadFirstScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            DataManager.Instance.SaveGameData();
            SceneManager.LoadScene("LoadingScene");
            StartCoroutine(LoadLevel("menu"));
        }
    }

    public void StopGame()
    {
        SetLevelDiamonds();
        LoadActiveScene();
    }

    private IEnumerator LoadLevel(string name)
    {
        yield return null;
        AsyncOperation load = SceneManager.LoadSceneAsync(name);
        while (!load.isDone)
            yield return null;
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
