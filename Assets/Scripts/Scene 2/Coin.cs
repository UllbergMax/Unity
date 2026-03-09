using UnityEngine;

public class Coin : MonoBehaviour
{
    public CoinManager manager;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger hit by: Player");

            manager.CollectCoin();

            Destroy(gameObject);
        }
    }
}