using UnityEngine;
using System.Collections;

public class Market : CreateSingletonGameObject<Market>
{
    private DateManager m_DateManager = new DateManager();
    private int m_Health = 0;
    private int m_MaxHealth = 5;
    private int timeSetHealth = 1;
    private string revTime;

    void OnEnable()
    {
        StartCoroutine(MarketCoroutine());//запускаем карутину, которая будет по таймауту отслеживать количесвто прошедшего времени
    }

    void OnDisable()
    {
        StopCoroutine(MarketCoroutine());//останавливаем
    }

    public void AddHealth(int count)//добавляем жизнь, если их ещё не максимум
    {
        if(m_Health < 5)
            m_Health = Mathf.Clamp(count,0,5);
        //print(m_Health);
    }

    public void SetCurrentDatePlayer() // устанавливаем текущее время
    {
        var value = m_DateManager.GetCurrentDateString();
        m_DateManager.SetPlayerDate(value);
    }


    IEnumerator MarketCoroutine()
    {
        while (true)
        {
            revTime = m_DateManager.GetPlayerDate("Date");
            var passedTime = m_DateManager.HowTimePassed(revTime, DateManager.DateType.minutes);
            if (passedTime >= 5)
            {
                AddHealth(passedTime / timeSetHealth); //делим прошедшие минуты на время создания одной жизни и берём целую часть от этого
                SetCurrentDatePlayer();
            }
            yield return new WaitForSeconds(30f);
        }
    }
}
