using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : CreateSingletonGameObject<GameController>
{

    private int currentLevel;

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
        Market.Instance.AddHealth(100);
        Market.Instance.Seeds += 50;
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
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadLevel(name));
    }

    public void LoadActiveScene()
    {
        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadLevel(name));
    }


    public void LoadFirstScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene("LoadingScene");
            StartCoroutine(LoadLevel("menu"));
        }
    }

    public void StopGame()
    {
        LoadActiveScene();
    }

    private IEnumerator LoadLevel(string name)
    {
        yield return null;
        AsyncOperation load = SceneManager.LoadSceneAsync(name);
        while (!load.isDone)
            yield return null;
    }


    void GetData()
    {
        if (Market.Instance != null)     //если маркет загружен
        {
            DataManager.Instance.LoadGameData();
        }
    }

    void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }
}
