using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public static int RubyCount;
    public static string RubyKey = "Rubies";
    public static int SeedsCount;
    //public static int LifesCount;
    public static int DiamondsCount;
    public static string DiamondKey = "Diamonds";
    public static bool inGame;
    


	// Use this for initialization
	void Start () {
        DiamondsCount = PlayerPrefs.GetInt(DiamondKey);
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public static void SaveBonus(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }
}
