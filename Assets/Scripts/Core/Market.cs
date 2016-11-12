using UnityEngine;
using System.Collections;

public class Market : CreateSingletonGameObject<Market>
{
    private DateManager m_DateManager = new DateManager();
    void Update()
    {

    }

    public void GetDayCount()
    {
        print(m_DateManager.HowTimePassed("01.11.2016", DateManager.DateType.day));
    }

}
