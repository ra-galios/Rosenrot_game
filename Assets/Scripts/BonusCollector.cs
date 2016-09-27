using UnityEngine;
using System.Collections;

public class BonusCollector : MonoBehaviour {
  
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D other)
    {
        Bonus bonus = other.gameObject.GetComponent<Bonus>();
        if (bonus)
        {
            bonus.GetBonus();
        }
    }
}
