using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {
	private void Start () {
	    if (PlayerPrefs.GetString("Tutorial") != "NotFirstStart")
	    {
	        PlayerPrefs.SetString("Tutorial", "NotFirstStart");
	    }
	    else
	    {
	        HideTutorial();
	    }
    }

    public void HideTutorial()
    {
        gameObject.GetComponent<Image>().enabled = false;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("Tutorial", "FirstStart");
    }
}
