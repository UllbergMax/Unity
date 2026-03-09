using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            BeeChaser.gameWon = true;

            int i = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(i + 1);
        }
    }
}