using UnityEngine;
using System.Collections;

public class Bonus_Lifes : Bonus {
    public int AddLifes = 1;
    public override void GetBonus()
    {
        Calculator.SetPlayerLifes(AddLifes);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
