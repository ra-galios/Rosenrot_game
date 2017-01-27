using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class EndGameTrigger : MonoBehaviour
{
    private bool endGame = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.gameObject.GetComponent<PlayerBehaviour>();

        if (player)
        {
            Destroy(player);
            StartCoroutine("StopGame");
        }
    }

    private IEnumerator StopGame()
    {
        yield return new WaitForSeconds(0.4f);
        GameController.Instance.FailGame();
    }
}
