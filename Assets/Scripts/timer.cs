using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class timer : MonoBehaviour {
    float Timer;
    public Text text;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Timer += Time.deltaTime;
        text.text = Timer.ToString("0.00");

        
    }


}
