using UnityEngine;
using System.Collections;

public class Market : CreateSingletonGameObject<Market>
{
    [SerializeField]
    private int m_Health;

    private DateManager m_DateManager = new DateManager();
    private string revTime;
    private int m_MaxHealth = 5;
    private int timeSetHealth = 5;
    [SerializeField]
    private int m_Seeds;//семечки
    private int m_Powder;//порох
    private int m_Star;//звезды

    public bool RunMarket() //нужен для инициализации сингтон-объекта на сцене
    {
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
        
    }
        
    void OnEnable()
    {
        Health = PlayerPrefs.GetInt("Health");//получаем сколько у нас было жизней ранее
        StartCoroutine(MarketCoroutine());//запускаем карутину, которая будет по таймауту отслеживать количесвто прошедшего времени
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("Health", Health);//сохраняем текущее значение жизней
        StopCoroutine(MarketCoroutine());//останавливаем
    }

    IEnumerator MarketCoroutine()
    {
        while (true)
        {
            revTime = m_DateManager.GetPlayerDate("Date");
            var passedTime = m_DateManager.HowTimePassed(revTime, DateManager.DateType.minutes);//сколько прошло времени
            if (passedTime >= timeSetHealth)//если прошло больше, чем время для начисления жизни
            {
                Health = passedTime / timeSetHealth; //делим прошедшие минуты на время создания одной жизни и берём целую часть от этого
                SetCurrentDatePlayer(); //ставим дату обновления жизней
            }
            yield return new WaitForSeconds(30f);//ждём 30 сек
        }
    }

    public void SetCurrentDatePlayer() // устанавливаем текущее время
    {
        var value = m_DateManager.GetCurrentDateString();
        m_DateManager.SetPlayerDate(value);
    }
    
    //свойства
    public int Health
    {
        get
        {
            return m_Health;
        }
        set
        {
            m_Health = Mathf.Clamp(value, 0, m_MaxHealth);//добавляем жизней, но не больше m_MaxHealth
        }
    }
    public int MaxHealth
    {
        get
        {
            return m_MaxHealth;
        }
        set
        {
            m_MaxHealth = value;
        }
    }
    public int TimeSetHealth
    {
        get
        {
            return timeSetHealth;
        }
        set
        {
            timeSetHealth = value;
        }
    }
    public int Seeds
    {
        get { return this.m_Seeds; }
        set { this.m_Seeds = value; }
    }
    public int Powder
    {
        get { return this.m_Powder; }
        set { m_Powder = value; }
    }
    public int Start
    {
        get { return this.m_Star; }
        set { this.m_Star = value; }
    }
}
