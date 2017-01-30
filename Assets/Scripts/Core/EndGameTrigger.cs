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
            AchievementsController.AddToAchievement(AchievementsController.Type.SelfDestructive, 1);
            AchievementsController.DiscardAchievement(AchievementsController.Type.SurvivorFinished);
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
