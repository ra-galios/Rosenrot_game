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
    private int m_Powder;//порох/бомбочка
    private int m_Dimond;//брилиант
    private int m_Ruby;//рубин
       
    void OnEnable()
    {
        if (!PlayerPrefs.HasKey("Health"))
        {
            PlayerPrefs.SetInt("Health", 5);
        }

        Health = PlayerPrefs.GetInt("Health");//получаем сколько у нас было жизней ранее

        StartCoroutine(MarketCoroutine());//запускаем карутину, которая будет по таймауту отслеживать количесвто прошедшего времени
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("Health", Health);//сохраняем текущее значение жизней 
    }

    IEnumerator MarketCoroutine()
    {
        while (Health < 5)
        {
            revTime = m_DateManager.GetPlayerDate("Date");
            var passedTime = m_DateManager.HowTimePassed(revTime, DateManager.DateType.minutes);//сколько прошло времени
            if (passedTime >= timeSetHealth)//если прошло больше, чем время для начисления жизни
            {
                Health = passedTime / timeSetHealth; //делим прошедшие минуты на время создания одной жизни и берём целую часть от этого, клампим, чтобы получить не больше 5
                Mathf.Clamp(Health, 0, m_MaxHealth);
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
    public int Powder
    {
        get { return this.m_Powder; }
        set { m_Powder = value; }
    }
    public int Dimond
    {
        get { return this.m_Dimond; }
        set { this.m_Dimond = value; }
    }
    public int Ruby
    {
        get { return this.m_Ruby; }
        set { this.m_Ruby = value; }
    }
}
