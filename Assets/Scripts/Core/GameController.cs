using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : CreateSingletonGameObject<GameController>
{

    private int currentLevel;

    public LevelData[] levelsData;
    public int totalGameLevels = 14;
    public PlayerBehaviour playerBeh = null;

    private Animator victoryPanelAnim;
    private Animator failPanelAnim;
    private Animator failDeadJacobAnim;

    private int diamondsCollectedOnLevel;
    private int rubiesCollectedOnLevel;
    private int seedsCollectedOnLevel;
    private int bombsCollectedOnLevel;

    // Use this for initialization
    void OnEnable()
    {
        GetData();

        SceneManager.sceneLoaded += ClearLocalBonuses;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= ClearLocalBonuses;
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

    public void LoadNextLevel()
    {
        if(levelsData[currentLevel].diamondsCollected == levelsData[currentLevel].isCollected.Length)
        {
            SceneManager.LoadScene("LoadingScene");
            StartCoroutine(LoadLevelAsync(SceneManager.GetSceneAt(SceneManager.GetActiveScene().buildIndex + 1).name));
        }
    }

    public void FailGame()
    {
        FailPanelAnim.SetTrigger("Fail");
        FailDeadJacobAnim.SetTrigger("Dead");
        PauseGame();
    }

    public void WinGame()
    {
        VictoryPanelAnim.SetTrigger("Fail");
        PauseGame();
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

    private void ClearLocalBonuses(Scene scene, LoadSceneMode mode)
    {
        DiamondsCollectedOnLevel = 0;
        RubiesCollectedOnLevel = 0;
        SeedsCollectedOnLevel = 0;
        BombsCollectedOnLevel = 0;
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

    public Animator VictoryPanelAnim
    {
        get { return victoryPanelAnim; }
        set { victoryPanelAnim = value; }
    }

    public Animator FailPanelAnim
    {
        get { return failPanelAnim; }
        set { failPanelAnim = value; }
    }

    public Animator FailDeadJacobAnim
    {
        get { return failDeadJacobAnim; }
        set { failDeadJacobAnim = value; }
    }

    public int DiamondsCollectedOnLevel
    {
        get { return diamondsCollectedOnLevel; }
        set { diamondsCollectedOnLevel = value; }
    }

    public int RubiesCollectedOnLevel
    {
        get { return rubiesCollectedOnLevel; }
        set { rubiesCollectedOnLevel = value; }
    }

    public int SeedsCollectedOnLevel
    {
        get { return seedsCollectedOnLevel; }
        set { seedsCollectedOnLevel = value; }
    }

    public int BombsCollectedOnLevel
    {
        get { return bombsCollectedOnLevel; }
        set { bombsCollectedOnLevel = value; }
    }
}
