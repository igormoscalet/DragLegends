using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    private int coinCount = 0;

    public TMP_Text coinCountText;

    public void MakeInAppPurchase(int coinPurchaseCount)
    {
        coinCount = PlayerPrefs.GetInt("coinCount", 0);

        // Increment coin count with new purchased coin count
        coinCount += coinPurchaseCount;

        // Save amount of player's coins in PlayerPrefs
        PlayerPrefs.SetInt("coinCount", coinCount);

        PlayerPrefs.Save();

        // Redraw UI
        ReDrawCoinCount();
    }

    public void ReDrawCoinCount()
    {
        int countToDraw = PlayerPrefs.GetInt("coinCount");
        coinCountText.text = "Coins: " + countToDraw.ToString();
    }
    void Start()
    {
        coinCount = PlayerPrefs.GetInt("coinCount");

        ReDrawCoinCount();
    }
}
