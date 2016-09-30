using UnityEngine;
using System.Collections;

public class PushgenBlock : MonoBehaviour {
    public float PushgenStartTime = 5f;

	// Use this for initialization
	void Start () {
        LevelGenerator2d.PushgenBlocks.Add(this);
        this.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
