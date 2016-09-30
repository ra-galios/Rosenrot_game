using UnityEngine;
using System.Collections;
using System;

public class Calculator : MonoBehaviour {
    //максимальное колличество жизней которое может быть у игрока за ожидание
    public static int maxLifes = 5;
    public static int GetPlayerLifes()
    {
        int currentLifes = PlayerPrefs.GetInt("PlayerLifes");
        int day = DateTime.Now.Day;
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;



        int addedLifes = 0;
        if(currentLifes < maxLifes)
        {
            if(day != PlayerPrefs.GetInt("LifeDay"))
            {
                addedLifes = maxLifes;
                PlayerPrefs.SetInt("LifeDay", day);
            }
            else if (hour > PlayerPrefs.GetInt("LifeHour"))
            {
                addedLifes = maxLifes;
                PlayerPrefs.SetInt("LifeHour", hour);
            }
            else
            {
                int prevMinute = PlayerPrefs.GetInt("LifeMinute");
                if(minute > prevMinute)
                {
                    addedLifes = (minute - prevMinute)/5;
                }
                PlayerPrefs.SetInt("LifeMinute", minute);
            }
            SetPlayerLifes(addedLifes);
        }

        return currentLifes;
    }

    public static void SetPlayerLifes(int add)
    {
        int currentLifes = PlayerPrefs.GetInt("PlayerLifes");
        int lifes = Mathf.Clamp(currentLifes + add, 0, maxLifes);
        PlayerPrefs.SetInt("PlayerLifes", lifes);
    }

}
