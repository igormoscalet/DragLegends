using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPManager : MonoBehaviour
{
    public Button upgradeButton;

    public TMP_Text levelText;

    public GameObject PurchasePanel;

    // Maximum Level of HP Possible
    public int maxLevel = 5;

    // levelPrices[0] - cost to upgrade to Level 2, etc...
    public List<int> levelPrices;

    // Start is called before the first frame update
    void Start()
    {
        // Initial setup
        if(!PlayerPrefs.HasKey("carHP"))
        {
            PlayerPrefs.SetInt("carHP", 1);
        }

        levelText.text = "Level " + PlayerPrefs.GetInt("carHP", 1).ToString(); 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void InitialUpgradeLevel()
    {
        // Get Current HP Level
        int currentLevel = PlayerPrefs.GetInt("carHP", 1);

        Debug.Log(currentLevel);

        // Change Purchase panel text
        PurchasePanel.SetActive(true);
        PurchasePanel.transform.Find("Purchase Text").GetComponent<TMP_Text>().text = "Do you want to upgrade to level " + (currentLevel + 1).ToString() + " for " + levelPrices[currentLevel - 1].ToString() + " coins?";


        // Reset Listeners to avoid charging again
        PurchasePanel.transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();

        // Check if user has enough money to upgrade HP
        if (PlayerPrefs.GetInt("coinCount") >= levelPrices[currentLevel - 1])
        {
            // Change button text if user have enough coins to buy HP
            PurchasePanel.transform.Find("Button").Find("Unlock").GetComponent<TMP_Text>().text = "Upgrade";
            // Add Listener to Upgrade button
            PurchasePanel.transform.Find("Button").GetComponent<Button>().onClick.AddListener(UpgradeLevel);
        }
        else
        {
            // Change button text if user have no enough coins to buy color
            PurchasePanel.transform.Find("Button").Find("Unlock").GetComponent<TMP_Text>().text = "Not Enough Coins";
        }
    }

    void UpgradeLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("carHP", 1);

        int currentCoinCount = PlayerPrefs.GetInt("coinCount", 0);

        PlayerPrefs.SetInt("coinCount", currentCoinCount - levelPrices[currentLevel - 1]);
        // Set new level and substract the level price from user coin count
        PlayerPrefs.SetInt("carHP", currentLevel + 1);

        GameObject.Find("CoinManager").GetComponent<CoinManager>().ReDrawCoinCount();

        // Hide Purchase Panel
        PurchasePanel.SetActive(false);

        levelText.text = "Level " + (currentLevel + 1).ToString();
        if (currentLevel + 1 == maxLevel)
        {
            // If level is maximum, don't show upgrade button and change with unactive "Maximum"
            upgradeButton.interactable = false;
            upgradeButton.transform.Find("Upgrade Text").GetComponent<TMP_Text>().text = "Maximum";
        }
    }
}
