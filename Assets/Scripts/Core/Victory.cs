using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour {
    private PlayerBehaviour player;

	// Update is called once per frame
	void Update ()                //!!!
    {
        if (GameController.Instance.playerBeh.IdLine == LevelGenerator.Instance.MaxLines)
        {
            print("Victory");
            GameController.Instance.WinGame();
        }
	}
}
