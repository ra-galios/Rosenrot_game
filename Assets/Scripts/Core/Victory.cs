using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour {
    private PlayerBehaviour player;
    private bool isVictory = false;

	// Update is called once per frame
	void Update ()                //!!!
    {
        if (GameController.Instance.playerBeh.IdLine == LevelGenerator.Instance.MaxLines && !isVictory)
        {
            GameController.Instance.WinGame();
            isVictory = true;
        }
	}
}
