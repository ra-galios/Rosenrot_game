using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : CreateSingletonGameObject<GameController>
{

    private int currentLevel;

    public LevelData[] levelsData;
    public int totalGameLevels = 14;
    public PlayerBehaviour playerBeh = null;

    [HideInInspector]
    public Animator VictoryPanelAnim;
    [HideInInspector]
    public Animator FailPanelAnim;


    // Use this for initialization
    void OnEnable()
    {
        GetData();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            LoadMainScene();
        }
    }

    public void LoadScene(string name)
    {
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadLevelAsync(name));
    }

    public void LoadActiveScene()
    {
        string name = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("LoadingScene");
        StartCoroutine(LoadLevelAsync(name));
    }


    public void LoadMainScene()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            SceneManager.LoadScene("LoadingScene");
            StartCoroutine(LoadLevelAsync("menu"));
        }
    }

    public void FailGame()
    {
        //FailPanelAnim.SetTrigger("Achievement");
        //PauseGame();
        LoadActiveScene();
    }

    public void WinGame()
    {
        LoadActiveScene();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    private IEnumerator LoadLevelAsync(string name)
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
