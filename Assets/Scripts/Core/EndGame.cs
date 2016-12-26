using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class EndGame : MonoBehaviour {
    private bool endGame=false;

    void OnTriggerEnter2D(Collider2D other)
    {
        var jumpPoint = other.gameObject.GetComponent<JumpPoint>() ? other.GetComponent<JumpPoint>() : null;
        var player = other.gameObject.GetComponent<PlayerBehaviour>() ? other.gameObject.GetComponent<PlayerBehaviour>() : null;

        if (player)
        {
            endGame = true;
        }

        if (jumpPoint)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehaviour>();
            if (jumpPoint.Line >= player.IdLine)
            {
                endGame = true;
            }
        }

        if (endGame)
        {
            print("Defeat");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
