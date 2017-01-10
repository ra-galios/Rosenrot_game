using UnityEngine;
using System.Collections;
using System;

public class Market : CreateSingletonGameObject<Market>
{
    public Action ChangeStatusAction;

    [SerializeField]
    private int m_Health;

    private DateManager m_DateManager = new DateManager();
    private string prevTime;
    private int m_MaxHealth = 5;
    private int timeSetHealth = 5;
    private int m_Seeds;//семечки
    private int m_Bomb;//порох/бомбочка
    [SerializeField]
    private int m_Dimond;//брилиант
    private int m_LocalDiamond;
    private int m_Ruby;//рубин
    private int m_MinutesUntilHealth;
    private int m_SecondsUntilHealth;
    private Coroutine marketCoroutine = null;

    void OnLevelWasLoaded()
    {
        m_LocalDiamond = 0;
    }

    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("Health"))
        {
            PlayerPrefs.SetInt("Health", 5);
        }
        PlayerPrefs.SetInt("Health", 2);
        Health = PlayerPrefs.GetInt("Health");//получаем сколько у нас было жизней ранее

        marketCoroutine = StartCoroutine(MarketCoroutine());//запускаем карутину, которая будет по таймауту отслеживать количесвто прошедшего времени
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("Health", Health);//сохраняем текущее значение жизней 
    }

    void Update()
    {
        if (Health < m_MaxHealth && marketCoroutine == null)
        {
            marketCoroutine = StartCoroutine(MarketCoroutine());
        }
    }

    IEnumerator MarketCoroutine()
    {
        while (Health < m_MaxHealth)
        {
            prevTime = m_DateManager.GetPlayerDate("Date");
            var passedTimeMinutes = m_DateManager.HowTimePassed(prevTime, DateManager.DateType.minutes);//сколько прошло времени
            var passedTimeSeconds = m_DateManager.HowTimePassed(prevTime, DateManager.DateType.seconds);//сколько прошло времени
            print((4 - passedTimeMinutes) + " : " + (59 - Mathf.Repeat(passedTimeSeconds, 60)));
            if (passedTimeMinutes >= timeSetHealth)//если прошло больше, чем время для начисления жизни
            {
                Health = passedTimeMinutes / timeSetHealth; //делим прошедшие минуты на время создания одной жизни и берём целую часть от этого, клампим, чтобы получить не больше 5
                Mathf.Clamp(Health, 0, m_MaxHealth);
                SetCurrentDatePlayer(); //ставим дату обновления жизней
            }
            yield return new WaitForSeconds(1f);//ждём 30 сек
        }
        StopCoroutine(marketCoroutine);
        marketCoroutine = null;
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
            m_Health = value;//добавляем жизней
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
    public int Bomb
    {
        get { return this.m_Bomb; }
        set { m_Bomb = value; }
    }
    public int Dimond
    {
        get { return this.m_Dimond; }
        set { this.m_Dimond = value; }
    }
    public int LocalDiamond
    {
        get { return this.m_LocalDiamond; }
        set { this.m_LocalDiamond = value; }
    }
    public int Ruby
    {
        get { return this.m_Ruby; }
        set { this.m_Ruby = value; }
    }

    public int MinutesUntilHealth
    {
        get { return this.m_MinutesUntilHealth; }
    }

    public int SecondsUntilHealth
    {
        get { return this.m_SecondsUntilHealth; }
    }
}
