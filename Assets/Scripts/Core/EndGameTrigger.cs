using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class EndGameTrigger : MonoBehaviour {
    private bool endGame = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        var jumpPoint = other.gameObject.GetComponent<JumpPoint>();
        var player = other.gameObject.GetComponent<PlayerBehaviour>();

        if (player)
        {
                GameController.Instance.StopGame();
        }

        if (jumpPoint)
        {
            player = GameController.Instance.playerBeh;
            if (jumpPoint.Line >= player.IdLine)
            {
                GameController.Instance.StopGame();
            }
        }
    }
}
