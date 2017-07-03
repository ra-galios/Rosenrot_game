using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class LevelAnalitics : MonoBehaviour
{

    // Use this for initialization
    public static void LevelVictory(int nm)
    {
		
        Analytics.CustomEvent("LevelVictory", new Dictionary<string, object>
		{
    		{"level_num", nm - 1}
    	});
		
    }

	public static void LevelDefeate(int nm)
    {
        Analytics.CustomEvent("LevelDefeate", new Dictionary<string, object>
		{
    		{"level_num", nm - 1}
    	});
		//print("Fail");
    }

}
