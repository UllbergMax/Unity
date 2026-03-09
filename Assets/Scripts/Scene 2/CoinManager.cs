using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public int coinsRemaining;

    void Start()
    {
        coinsRemaining = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void CollectCoin()
    {
        coinsRemaining--;

        if (coinsRemaining <= 0)
        {
            int i = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(i + 1);
        }
    }
}