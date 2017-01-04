using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour {
    private PlayerBehaviour player;
    private int maxLines;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
        maxLines = LevelGenerator.Instance.MaxLines;
    }

	// Update is called once per frame
	void Update () {
        if (player.IdLine == maxLines)
        {
            print("Victory");
            GameController.Instance.StopGame();
        }
	}
}
