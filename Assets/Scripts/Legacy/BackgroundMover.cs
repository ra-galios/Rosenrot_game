using UnityEngine;
using System.Collections;

public class BackgroundMover : MonoBehaviour {
    public float FlowsSpeed;
    float offset;

    // Update is called once per frame
    void Update()
    {
        FlowsSpeed = LevelGenerator.Instance.SpeedPusher/10;
        if (LevelGenerator.Instance.IsRunLevel)
        {
            offset = Time.deltaTime * FlowsSpeed + offset;
            GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }
    }
}
