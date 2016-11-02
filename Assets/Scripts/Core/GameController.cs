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

    public static GameController Instance;

    void Awake(){
        if (Instance == null)
        {
          Instance = this;  
        }
    }

	// Use this for initialization
	void Start () {
        DiamondsCount = PlayerPrefs.GetInt(DiamondKey);
	}
	
    public static void SaveBonus(string key, int _value)
    {
        PlayerPrefs.SetInt(key, _value);
    }
}
