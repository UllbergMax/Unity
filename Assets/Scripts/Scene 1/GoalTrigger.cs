using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public GameObject bee;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (bee != null)
                bee.SetActive(false);

            int i = SceneManager.GetActiveScene().buildIndex;

            if (i + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(i + 1);
            }
            else
            {
                Debug.Log("Game completed!");
            }
        }
    }
}