using UnityEngine;
using System.Collections;

public class anima : MonoBehaviour {
    Animator idle;

    // Use this for initialization
    void Start () {
        idle = GetComponent<Animator>();
        idle.SetBool("start", false);
      
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            idle.SetBool("start", true);
                    }
        if (PlayerBehavior2d.isFalling == true)
            {
            idle.SetBool("falling", true);
        }
        }
}
