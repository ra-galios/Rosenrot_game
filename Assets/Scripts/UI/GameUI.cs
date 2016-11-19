using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField]
    Text m_LifesCounter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_LifesCounter.text = Market.Instance.Health.ToString();
	}
}
