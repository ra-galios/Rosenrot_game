using UnityEngine;
using System.Collections;

public class BackgroundMover : MonoBehaviour {
    private float FlowsSpeed;
    private float offset;
    private Renderer m_BackgroundRenderer;


    private void Start()
    {
        m_BackgroundRenderer = GetComponent<Renderer>();
        offset = m_BackgroundRenderer.material.GetTextureOffset("_MainTex").y;
    }
    // Update is called once per frame
    void Update()
    {
        FlowsSpeed = LevelGenerator.Instance.SpeedPusher/10;
        if (LevelGenerator.Instance.IsRunLevel)
        {
            offset = Time.deltaTime * FlowsSpeed + offset;
            m_BackgroundRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
        }
    }
}
