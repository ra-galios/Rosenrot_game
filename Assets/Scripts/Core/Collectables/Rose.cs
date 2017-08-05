using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rose : CollectableGO
{
    private Victory m_VictoryObj;

    private void Start()
    {
        m_VictoryObj = FindObjectOfType<Victory>();
    }

    public override void EnterBonus()
    {
        m_VictoryObj.endGame();
        GameController.Instance.ResumeGame();
        GameController.Instance.LoadScene("Final_moving");
    }
}


