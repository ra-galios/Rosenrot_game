using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//хранение ачивок
//если 0 - не бралась ачика
//больше 0 - значение ачивки
//меньше 0 - ачивка получена

public static class AchievementsController
{
    public enum Type
    {
        //ачивки без уровня
        GoodStart = 0,
        RockyRoad = 1,
        DoingGreyt = 2,
        LetItBee = 3,
        AbyssGazesIntoYou = 4,
        BeOurGuest = 5,
        //ачивки по уровням - на каждую заводится по 3 номера
        AlwaysRight = 6,
        ExtremeLeft = 9,
        SelfDestructive = 12,
        HeartBreaker = 15,
        SurvivorFinished = 18,
        EverybodysBestFriend = 21,
        RubyRubyRubyRuby = 24,
        JacobAndTheBeanstalk = 27,
        ExplosiveBehavior = 30,
        Catchy = 33,
        BigBang = 36,
        NeverGiveUp = 39,
        GotHigh = 42,
        Adept = 45,
    }

    public enum RewardType {Diamonds, Rubies, Seeds, Bombs, Health}


    public static void AddToAchievement(Type type, int addValue)
    {
        int val = GetAchievement(type);
        DataManager.Instance.SetAchievement((int)type, val + addValue);

        AddToListAchievement(type, val, val + addValue);
    }

    private static void AddToListAchievement(Type type, int prevVal, int curVal)        //проверить не достигли ли ачивки, добавить в лист на вывод если достигли
    {
        int numberInResourcePrefab = -1;
        Achievement achievementInform = FindAchievementInResources(type, ref numberInResourcePrefab);

        for (int j = 0; j < achievementInform.m_NeedToAchieve.Length; j++)
        {
            if (curVal >= achievementInform.m_NeedToAchieve[j] && prevVal < achievementInform.m_NeedToAchieve[j])
            {
                if (!GameController.Instance.AchievementsToShow.Contains(numberInResourcePrefab))
                    GameController.Instance.AchievementsToShow.Add(numberInResourcePrefab);      //добавить номер награды в рeсурсе
                break;
            }
        }
    }

    public static Achievement FindAchievementInResources(Type type, ref int numberInResourcePrefab)
    {
        for (int i = 0; i < GameController.Instance.AchievementRevards.Achievements.Length; i++)
        {
            if (GameController.Instance.AchievementRevards.Achievements[i].m_Achievement == type)
            {
                Achievement achievementInform = GameController.Instance.AchievementRevards.Achievements[i]; //нашли информацию по ачивке
                numberInResourcePrefab = i;
                return achievementInform;
            }
        }
        Debug.LogError("Not Found Achievement of Type: " + type + " in Resources/Achievements");
        return new Achievement();
    }

    public static void CompleteAchievement(Type type)       //пока лишнее
    {
        DataManager.Instance.SetAchievement((int)type, -1);
    }

    public static int GetAchievement(Type type)      //текущее значение ачивки
    {
        return DataManager.Instance.GetAchievement((int)type);
    }

    public static bool CheckAchievement(Type type, int currentValue)    //пока лишнее
    {
        //возвращает true только если ачивка была взята в первый раз
        int val = GetAchievement(type);
        if (val > 0 && currentValue >= val)
        {
            CompleteAchievement(type);
            return true;
        }
        return false;
    }


    public static bool CheckAchievementComplete(Type type, int level = 0)       //пока лишнее
    {
        if (GetAchievement(type) < 0)
        {
            return true;
        }
        return false;
    }

    public static void GetRevard(Type type, out int revard)     //лишнее
    {
        revard = 0;
    }

    public static void GetRevard(Type type, out int[] revards)  //получить награды (добавить логику)
    {
        revards = new int[3] { 0, 0, 0 };
    }
}
