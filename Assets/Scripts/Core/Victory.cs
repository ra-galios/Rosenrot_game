using UnityEngine;
using System;
using System.Collections.Generic;

public class Victory : MonoBehaviour
{

    //public static List<int> achievementsToShow = new List<int>();
    public AchievementUI_Base LevelAchievementPanel;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AchievementsController.AddToAchievement(AchievementsController.Type.RubyRubyRubyRuby, 1);
            AchievementsController.AddToAchievement(AchievementsController.Type.EverybodysBestFriend, 1);
            //DataManager.Instance.SetAchievement((int)AchievementsController.Type.RubyRubyRubyRuby, 0);
            //DataManager.Instance.SetAchievement((int)AchievementsController.Type.EverybodysBestFriend, 0);

            print("showList length: " + GameController.Instance.AchievementsToShow.Count);

            print("ach value: " + AchievementsController.GetAchievement(AchievementsController.Type.RubyRubyRubyRuby));
        }
    }

    void CheckWin(int playerIdLine)
    {
        if (playerIdLine == LevelGenerator.Instance.MaxLines)
        {
            Debug.Log("WinGame");
            AchievementsController.DiscardAchievement(AchievementsController.Type.SelfDestructive);
            AchievementsController.AddToAchievement(AchievementsController.Type.SurvivorFinished, 1);

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

            if (LevelAchievementPanel)
                LevelAchievementPanel.Show();
            else
                GameController.Instance.WinGame();
        }
    }
}