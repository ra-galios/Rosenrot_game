using UnityEngine;
using System.Collections;

public class BackgoundTransit : MonoBehaviour
{
    private float FlowsSpeed;
    private float[] offset = new float[3];
    [SerializeField]
    private Renderer[] m_BackgroundRenderer = new Renderer[3];
    [SerializeField]
    private Texture[] m_TransitionTex = new Texture[2];
    [SerializeField]
    private Texture[] m_BGTex = new Texture[2];
    public bool m_Transition;
    public int m_TransitionIndex;
    public bool m_InTransition = false;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            m_BackgroundRenderer[i] = m_BackgroundRenderer[i].GetComponent<Renderer>();
            offset[i] = m_BackgroundRenderer[i].material.GetTextureOffset("_MainTex").y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FlowsSpeed = LevelGenerator.Instance.SpeedPusher / 10;
        if (LevelGenerator.Instance.IsRunLevel)
        {
            for (int i = 0; i < 3; i++)
            {
                offset[i] += Time.deltaTime * FlowsSpeed;
                m_BackgroundRenderer[i].material.SetTextureOffset("_MainTex", new Vector2(0, offset[i]));
            }

            if (m_Transition && m_TransitionIndex < 2 && !m_InTransition)
            {
                StartCoroutine(MakeTransition1());
                m_InTransition = true;
                m_Transition = false;
            }
        }
    }

    private IEnumerator MakeTransition1()
    {
        while ((offset[1] - (int)offset[1]) < 0.84f || (offset[1] - (int)offset[1]) > 0.95f)
        {
            yield return null;
        }
        m_BackgroundRenderer[1].material.SetTexture("_MainTex", m_TransitionTex[m_TransitionIndex]);
        StartCoroutine(MakeTransition2());
        while ((offset[1] - (int)offset[1]) < 0.37f || (offset[1] - (int)offset[1]) > 0.48f)
        {
            yield return null;
        }
        m_BackgroundRenderer[1].material.SetTexture("_MainTex", m_BGTex[m_TransitionIndex]);
    }
    private IEnumerator MakeTransition2()
    {
        while ((offset[2] - (int)offset[2]) > 0.09f)
        {
            yield return null;
        }
        m_BackgroundRenderer[2].material.SetTexture("_MainTex", m_TransitionTex[m_TransitionIndex]);
        StartCoroutine(MakeTransition0());
        while ((offset[2] - (int)offset[2]) < 0.54f || (offset[2] - (int)offset[2]) > 0.64f)
        {
            yield return null;
        }
        m_BackgroundRenderer[2].material.SetTexture("_MainTex", m_BGTex[m_TransitionIndex]);
    }
    private IEnumerator MakeTransition0()
    {
        while ((offset[0] - (int)offset[0]) > 0.1f)
        {
            yield return null;
        }
        m_BackgroundRenderer[0].material.SetTexture("_MainTex", m_TransitionTex[m_TransitionIndex]);
        while ((offset[0] - (int)offset[0]) < 0.56f || (offset[0] - (int)offset[0]) > 0.66f)
        {
            yield return null;
        }
        m_BackgroundRenderer[0].material.SetTexture("_MainTex", m_BGTex[m_TransitionIndex]);
        m_InTransition = false;
        m_TransitionIndex++;
    }
}
