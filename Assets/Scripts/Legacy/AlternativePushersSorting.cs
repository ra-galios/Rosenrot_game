using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AlternativePushersSorting : MonoBehaviour {

    public static  List<List<GameObject>> AlternativePushersList = new List<List<GameObject>>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (LevelGenerator2d.AlternativePushers == null 
            && AlternativePushersList.Count > 0 
            && AlternativePushersList[0] != null)
        {
            LevelGenerator2d.AlternativePushers = AlternativePushersList[0];
            AlternativePushersList.Remove(AlternativePushersList[0]);
        }
    }

    public static void AddToAlternativePushersList(List<GameObject> m_List)
    {
        if (m_List != null)
        {
            if (LevelGenerator2d.AlternativePushers == null)
            {
                LevelGenerator2d.AlternativePushers = m_List;
            }
            else
            {
                AlternativePushersList.Add(m_List);
            }
        }
        

    }
}
