using UnityEngine;
using System.Collections;

public class Market : CreateSingletonGameObject<Market>
{
    [SerializeField]
    private int m_Health;

    private DateManager m_DateManager = new DateManager();
    private int m_MaxHealth = 5;
    private int timeSetHealth = 5;  //минут до восполнения жизни
    private int m_Seeds;//семечки
    private int m_Bomb;//порох/бомбочка
    [SerializeField]
    private int m_Dimond;//брилиант
    private int m_Ruby;//рубин
    private int m_MinutesUntilHealth;
    private float m_SecondsUntilHealth;
    private const float secondsInMinute = 60f;
    private Coroutine timerCoroutine = null;
    private int curentlyAddHealth = 0;

    void Start()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (Health < m_MaxHealth)
        {
            string timeChangeHealth = m_DateManager.GetPlayerDate("Date");

            if (timeChangeHealth != null && timeChangeHealth != "")
            {
                int passedMinutes = m_DateManager.HowTimePassed(timeChangeHealth, DateManager.DateType.minutes);        //прошло минут с прошедшего запуска
                int passedSeconds = m_DateManager.HowTimePassed(timeChangeHealth, DateManager.DateType.seconds);
                //print(m_Health + " " + ((int)passedMinutes / TimeSetHealth - curentlyAddHealth));

                Health += (int)passedMinutes / TimeSetHealth - curentlyAddHealth;
                curentlyAddHealth = (int)passedMinutes / TimeSetHealth;
                Health = Health > m_MaxHealth ? m_MaxHealth : Health;

                if (Health < m_MaxHealth)
                {
                    m_MinutesUntilHealth = (timeSetHealth - 1) - passedMinutes % timeSetHealth;         //осталось минут до пополнения жизней
                    m_SecondsUntilHealth = secondsInMinute - passedSeconds % secondsInMinute;
                    if (timerCoroutine == null)
                    {
                        timerCoroutine = StartCoroutine(CountdownTimer());
                    }
                }
                else
                {
                    ResetTimer();
                }
            }
            else
            {
                timeChangeHealth = m_DateManager.GetCurrentDateString();    //время начала отсчета таймера
                m_DateManager.SetPlayerDate(timeChangeHealth);      //сохраняем время
                m_MinutesUntilHealth = timeSetHealth - 1;
                m_SecondsUntilHealth = secondsInMinute;
                curentlyAddHealth = 0;
                if (timerCoroutine == null)
                {
                    timerCoroutine = StartCoroutine(CountdownTimer());
                }
            }
        }
        else
        {
            ResetTimer();
        }
    }

    private IEnumerator CountdownTimer()
    {
        while (Health < m_MaxHealth)
        {
            if (m_SecondsUntilHealth <= 0)
            {
                m_SecondsUntilHealth = secondsInMinute;
                m_MinutesUntilHealth--;
                if (m_MinutesUntilHealth < 0)
                {
                    m_MinutesUntilHealth = timeSetHealth - 1;
                    Health++;
                }
            }
            m_SecondsUntilHealth -= Time.unscaledDeltaTime;
            //print(m_MinutesUntilHealth + " : " + (int)m_SecondsUntilHealth);
            yield return null;
        }

        if (Health >= m_MaxHealth)
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        curentlyAddHealth = 0;
        m_MinutesUntilHealth = 0;
        m_SecondsUntilHealth = 0;
        m_DateManager.SetPlayerDate(null);
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
            //CheckHealth();
        }
    }

    public int CurentlyAddHealth
    {
        get
        {
            return this.curentlyAddHealth;
        }
        set
        {
            curentlyAddHealth = value;
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
        get { return (int)this.m_SecondsUntilHealth; }
    }
}
