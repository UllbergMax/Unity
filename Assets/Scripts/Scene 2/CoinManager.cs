using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public int totalCoins;
    int collected;

    public void CollectCoin()
    {
        collected++;
        Debug.Log("Coins: " + collected + " / " + totalCoins);

        if (collected >= totalCoins)
        {
            Debug.Log("All coins collected! Scene 2 complete!");
        }
    }
}