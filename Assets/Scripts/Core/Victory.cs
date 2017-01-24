using UnityEngine;
using System;
using System.Collections.Generic;

public class Victory : MonoBehaviour
{

    //public static List<int> achievementsToShow = new List<int>();

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
            //DataManager.Instance.SetAchievement((int)AchievementsController.Type.RubyRubyRubyRuby, 0);
            print("showList length: " + GameController.Instance.AchievementsToShow.Count);
            if (GameController.Instance.AchievementsToShow.Count == 1)
                print("showList value: " + GameController.Instance.AchievementsToShow[0]);
            print("ach value: " + AchievementsController.GetAchievement(AchievementsController.Type.RubyRubyRubyRuby));
        }
    }

    void CheckWin(int playerIdLine)
    {
        if (playerIdLine == LevelGenerator.Instance.MaxLines)
        {
            // GameObject achievementsPrefab = Resources.Load("Achievements", typeof(GameObject)) as GameObject;
            // AchievementRevards achievementRevards = achievementsPrefab.GetComponent<AchievementRevards>();

            // int achievementsLength = DataManager.Instance.GetAchievementsDataLength();

            // AchievementsController.Type type = (AchievementsController.Type)0;
            // int level = 0;

            // for (int i = 0; i < achievementsLength; i++)
            // {
            //     if (Enum.IsDefined(typeof(AchievementsController.Type), i))
            //     {
            //         type = (AchievementsController.Type)i;
            //         level = 0;
            //     }
            //     else
            //     {
            //         level++;
            //     }

            //     int achievementValue = AchievementsController.GetAchievement(type, level);
            //     for (int j = 0; j < achievementRevards.Achievements.Length; j++)
            //     {
            //         if (achievementRevards.Achievements[j].m_Achievement == type)
            //         {
            //             if (achievementValue > 0 && achievementValue >= achievementRevards.Achievements[j].m_NeedToAchieve[level])
            //             {
            //                 //
            //             }
            //             break;
            //         }
            //     }


            // }

            GameController.Instance.WinGame();
        }
    }
}
