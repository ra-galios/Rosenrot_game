using UnityEngine.UI;
using UnityEngine;

public class BonusPanelUI : MonoBehaviour {

	private Animator m_Animator;

	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		Victory.m_OnStartBonusLevelAction += PlayBonusLevelAnimation;
	}

	private void OnDisable()
	{
		Victory.m_OnStartBonusLevelAction -= PlayBonusLevelAnimation;
	}

	private void PlayBonusLevelAnimation()
	{
		m_Animator.SetTrigger("bonuslevel");
	}
}
