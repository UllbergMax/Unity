using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    float startTime;

    public GameObject missionCompleteText;

    void Start()
    {
        startTime = Time.time;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Ignore collisions for first second after spawning
        if (Time.time < startTime + 1f)
            return;

        if (collision.gameObject.CompareTag("Pluto"))
        {
            // Hide player
            gameObject.SetActive(false);

            // Show victory text
            missionCompleteText.SetActive(true);

            // Freeze the game
            Time.timeScale = 0f;
        }
        else if (collision.gameObject.CompareTag("Planet"))
        {
            // Restart current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}