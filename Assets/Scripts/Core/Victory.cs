using UnityEngine;
using System;

public class Victory : MonoBehaviour
{

    private AchievementUI_Leveled LevelAchievementPanel;
    private bool m_OnBonusLevel;

    public static Action m_OnStartBonusLevelAction;

    private void Start()
    {
        GameController.Instance.CheckOnBonusLevel();
        LevelAchievementPanel = FindObjectOfType<AchievementUI_Leveled>();
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
            m_OnStartBonusLevelAction.Invoke();
            m_OnBonusLevel = true;
        }
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