using UnityEngine;
using System.Collections;

public class Market : CreateSingletonGameObject<Market>
{
    static DateManager dateManager;

    // Use this for initialization
    void Start () {
        var temp = dateManager.HowTimePassed("01.11.2016", DateManager.DateType.day);
        print(temp);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
