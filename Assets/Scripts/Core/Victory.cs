using UnityEngine;
using System.Collections;

public class Victory : MonoBehaviour
{

    private AchievementUI_Leveled LevelAchievementPanel;
    private Animator m_BonusPanelAnim;
    private bool m_OnBonusLevel;

    private void Start()
    {
        GameController.Instance.CheckOnBonusLevel();
        LevelAchievementPanel = FindObjectOfType<AchievementUI_Leveled>();
        GameObject bonusObj = GameObject.Find("Image_Bonus_LEVEL");
        if (bonusObj)
            m_BonusPanelAnim = bonusObj.GetComponent<Animator>();
    }

    void OnEnable()
    {
        PlayerBehaviour.PlayerChangeLine += CheckWin;
    }

    void OnDisable()
    {
        PlayerBehaviour.PlayerChangeLine -= CheckWin;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     AchievementsController.AddToAchievement(AchievementsController.Type.RubyRubyRubyRuby, 1);
        //     AchievementsController.AddToAchievement(AchievementsController.Type.EverybodysBestFriend, 1);
        //     //DataManager.Instance.SetAchievement((int)AchievementsController.Type.RubyRubyRubyRuby, 0);
        //     //DataManager.Instance.SetAchievement((int)AchievementsController.Type.EverybodysBestFriend, 0);

        //     print("showList length: " + GameController.Instance.AchievementsToShow.Count);

        //     print("ach value: " + AchievementsController.GetAchievement(AchievementsController.Type.RubyRubyRubyRuby));
        // }
    }

    void CheckWin(int playerIdLine)
    {
        if (playerIdLine == LevelGenerator.Instance.MaxLines)
        {
            SetWin();
        }
        else if (playerIdLine == LevelGenerator.Instance.LastRockId && Market.Instance.Seeds > 0)
        {
            Debug.Log("BonusLevel");
            m_OnBonusLevel = true;
            if (m_BonusPanelAnim)
                StartCoroutine(BonusLevelCoroutine());
        }
    }

    private IEnumerator BonusLevelCoroutine()
    {
        m_BonusPanelAnim.SetTrigger("bonuslevel");
        //GameController.Instance.PauseGame();
        yield return new WaitForSecondsRealtime(1f);
        //GameController.Instance.ResumeGame();
    }

    private void CheckLeveledAchievements()
    {
        AchievementsController.DiscardAchievement(AchievementsController.Type.SelfDestructive);
        AchievementsController.AddToAchievement(AchievementsController.Type.Survivor, 1);

        if (GameController.Instance.LevelsData[GameController.Instance.CurrentLevel].diamondsCollected == GameController.Instance.DiamondsOnLevel)
        {
            if (GameController.Instance.AchievementRevards.Achievements[(int)AchievementsController.Type.GoodStart].m_NeedToAchieve[0] == GameController.Instance.CurrentLevel)
            {
                AchievementsController.AddToAchievement(AchievementsController.Type.GoodStart, GameController.Instance.CurrentLevel);
                AchievementsController.AddToAchievement(AchievementsController.Type.GoodStart, 1);
            }
            if (GameController.Instance.AchievementRevards.Achievements[(int)AchievementsController.Type.RockyRoad].m_NeedToAchieve[0] == GameController.Instance.CurrentLevel)
            {
                AchievementsController.AddToAchievement(AchievementsController.Type.RockyRoad, GameController.Instance.CurrentLevel);
            }
            if (GameController.Instance.AchievementRevards.Achievements[(int)AchievementsController.Type.DoingGreyt].m_NeedToAchieve[0] == GameController.Instance.CurrentLevel)
            {
                AchievementsController.AddToAchievement(AchievementsController.Type.DoingGreyt, GameController.Instance.CurrentLevel);
            }
            if (GameController.Instance.AchievementRevards.Achievements[(int)AchievementsController.Type.LetItBee].m_NeedToAchieve[0] == GameController.Instance.CurrentLevel)
            {
                AchievementsController.AddToAchievement(AchievementsController.Type.LetItBee, GameController.Instance.CurrentLevel);
            }
            if (GameController.Instance.AchievementRevards.Achievements[(int)AchievementsController.Type.CameToTheDarkSide].m_NeedToAchieve[0] == GameController.Instance.CurrentLevel)
            {
                AchievementsController.AddToAchievement(AchievementsController.Type.CameToTheDarkSide, GameController.Instance.CurrentLevel);
            }
        }
    }

    public void SetWin()
    {
        Debug.Log("WinGame");
        CheckLeveledAchievements();

        if (LevelAchievementPanel)
            LevelAchievementPanel.ShowBonusPanel(true);
        else
        {
            GameController.Instance.WinGame();
        }
    }

    public bool OnBonusLevel
    {
        get { return m_OnBonusLevel; }
        set { m_OnBonusLevel = value; }
    }
}