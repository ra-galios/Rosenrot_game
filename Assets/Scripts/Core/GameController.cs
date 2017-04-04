using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameController : CreateSingletonGameObject<GameController>
{
    private int currentLevel;
    private int diamondsOnLevel;

    [SerializeField]
    private LevelData[] levelsData;
    [SerializeField]
    private int totalGameLevels = 10;
    [SerializeField]
    private PlayerBehaviour playerBeh = null;

    private Animator victoryPanelAnim;
    private Animator failPanelAnim;
    private Animator failDeadJacobAnim;

    private static List<int> achievementsToShow = new List<int>();
    private int diamondsCollectedOnLevel;
    private int rubiesCollectedOnLevel;
    private int seedsCollectedOnLevel;
    private int bombsCollectedOnLevel;
    private AchievementRevards achievementRevards;
    public static Action m_VictoryAction;
    public static Action m_FailAction;
    public int m_DiesInARow;

    [SerializeField]
    private bool onBonusLevel = false;
    private DateManager m_DateManager = new DateManager();

    private void LoadResources()
    {
        GameObject achievementsPrefab = Resources.Load("Achievements", typeof(GameObject)) as GameObject;
        achievementRevards = achievementsPrefab.GetComponent<AchievementRevards>();
    }

    private void Start()
    {
        ExceptionLogger.Instance.Create();
        m_DiesInARow = 0;
        AdeptAchievement();
    }

    private void AdeptAchievement()
    {
        string lastEnter = m_DateManager.GetPlayerDate("AdeptAchievement");

        if (lastEnter != "")
        {
            int days = m_DateManager.HowTimePassed(lastEnter, DateManager.DateType.day);
            if (days == 1)
            {
                m_DateManager.SetDate("AdeptAchievement", m_DateManager.GetCurrentDateString());
                AchievementsController.AddToAchievement(AchievementsController.Type.Adept, 1);
            }
            else if (days > 1)
            {
                m_DateManager.SetDate("AdeptAchievement", m_DateManager.GetCurrentDateString());
                AchievementsController.DiscardAchievement(AchievementsController.Type.Adept);
                AchievementsController.AddToAchievement(AchievementsController.Type.Adept, 1);
            }
        }
        else
        {
            m_DateManager.SetDate("AdeptAchievement", m_DateManager.GetCurrentDateString());
            AchievementsController.AddToAchievement(AchievementsController.Type.Adept, 1);
        }
    }

    private void OnEnable()
    {
        GetData();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        SceneManager.sceneLoaded += ClearLocalBonuses;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= ClearLocalBonuses;
    }

    public void LoadScene(string name)
    {
        m_DiesInARow = 0;
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
            m_DiesInARow = 0;
            SceneManager.LoadScene("LoadingScene");
            StartCoroutine(LoadLevelAsync("menu"));
        }
    }

    public void LoadNextLevel()
    {
        if (LevelsData[currentLevel].diamondsCollected == LevelsData[currentLevel].IsCollected.Length)
        {
            m_DiesInARow = 0;
            currentLevel++;
            SceneManager.LoadScene("LoadingScene");
            StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    public void FailGame()
    {
        LevelAnalitics.LevelDefeate(SceneManager.GetActiveScene().buildIndex);
        FailPanelAnim.SetTrigger("Fail");
        FailDeadJacobAnim.SetTrigger("Dead");
        m_FailAction.Invoke();
        PauseGame();
        
    }

    public void WinGame()
    {
         LevelAnalitics.LevelVictory(SceneManager.GetActiveScene().buildIndex);
        VictoryPanelAnim.SetTrigger("Win");
        m_VictoryAction.Invoke();
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
        //CheckOnBonusLevel();
        AsyncOperation load = SceneManager.LoadSceneAsync(name);
        while (!load.isDone)
            yield return null;
    }

    private IEnumerator LoadLevelAsync(int index)
    {
        yield return null;
        //CheckOnBonusLevel();
        AsyncOperation load = SceneManager.LoadSceneAsync(index);
        while (!load.isDone)
            yield return null;
    }

    private void GetData()
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

    private void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
    }

    private void OnApplicationPause(bool isPause)
    {
        DataManager.Instance.SaveGameData();
    }

    public void CheckOnBonusLevel()
    {
        // print(LevelsData[GameController.Instance.CurrentLevel].diamondsCollected + " == " + LevelsData[GameController.Instance.CurrentLevel].IsCollected.Length);
        // if (LevelsData[GameController.Instance.CurrentLevel].diamondsCollected == LevelsData[GameController.Instance.CurrentLevel].IsCollected.Length)
        // {
        //     onBonusLevel = true;
        // }
        // else
        // {
        //     onBonusLevel = false;
        // }
        onBonusLevel = true; //бонусный уровень доступен вне зависимости от собранных алмазов
    }

    public int CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; }
    }

    public int DiamondsOnLevel
    {
        get { return diamondsOnLevel; }
        set { diamondsOnLevel = value; }
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

    public List<int> AchievementsToShow
    {
        get { return achievementsToShow; }
        set { achievementsToShow = value; }
    }

    public PlayerBehaviour PlayerBeh
    {
        get { return playerBeh; }
        set { playerBeh = value; }
    }

    public int TotalGameLevels
    {
        get { return totalGameLevels; }
    }

    public LevelData[] LevelsData
    {
        get { return levelsData; }
        set { levelsData = value; }
    }

    public AchievementRevards AchievementRevards
    {
        get
        {
            if (achievementRevards)
            {
                return achievementRevards;
            }
            else
            {
                LoadResources();
                return achievementRevards;
            }
        }
    }

    public bool OnBonusLevel
    {
        get { return onBonusLevel; }
        set { onBonusLevel = value; }
    }
}
