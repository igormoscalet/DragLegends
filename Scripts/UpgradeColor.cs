using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UpgradeColor : MonoBehaviour
{
    // Car Color object
    [System.Serializable]
    public class CarColor{
        public string name;
        public Color color;
        public int price;
    }

    // We need an array because we want to change color of all parts for our car mesh (e.g Body / Spoiler). Also we have different LOD's for every part.
    public GameObject[] CarPaint;

    // Get car animator to play Color Change animation
    public Animator carAnimator;

    // Get AudioSource to play Color Change sound
    public AudioSource carAudioSource;

    // Get GameObject for Purchase window canvas
    public GameObject PurchasePanel;

    // Get GameObject for colors panel
    public GameObject ColorsPanel;

    // We set our color manually in Prefab to make it appear better visually and customize
    public List<CarColor> CarColors;

    // Delay to change car color that is dependant on Movable Part Lift animation
    public float animationDelay;

    // Keep current offer color name so we can find it when click button
    private string currentOfferName;

    // Keep current offer price
    private int currentOfferPrice;

    IEnumerator ChangeColorAfterDelay(Color selectedColor, string colorName) {
        
        // Wait animationDelay seconds
        yield return new WaitForSeconds(animationDelay);

        // Iterate through every LOD in Car GameObjects array
        foreach (GameObject carPaint in CarPaint)
        {
            // Create a new Material Property Block
            var block = new MaterialPropertyBlock();

            // Set URP Base Color to needed color
            block.SetColor("_BaseColor", selectedColor);

            // Set Property Block to our Car Mesh Renderer
            carPaint.GetComponent<Renderer>().SetPropertyBlock(block);

            // Save Info About Car Color
            PlayerPrefs.SetString("carColor", colorName);
        }
    }

    void ChangeColor(CarColor selectedColor)
    {
        // Change Color after animationDelay
        StartCoroutine(ChangeColorAfterDelay(selectedColor.color, selectedColor.name));
    }

    void InitiatePurchase(string color, int price)
    {
        // Get current money
        int currentCoinCount = PlayerPrefs.GetInt("coinCount");
        
        // Set money - purchased color price
        PlayerPrefs.SetInt("coinCount", currentCoinCount - price);

        // Redraw money UI
        GameObject.Find("CoinManager").GetComponent<CoinManager>().ReDrawCoinCount();

        // Set color to unlocked (forever)
        PlayerPrefs.SetInt(color + "Unlocked", 1);

        // Capitalize color (e.g red => Red) and find Lock GameObject in hierarchy to hide it
        Transform LockPanel = ColorsPanel.transform.Find(char.ToUpper(color[0]) + color[1..]).Find("Lock");

        // Hide lock
        LockPanel.gameObject.SetActive(false);

        // Hide Purchase Popup
        PurchasePanel.SetActive(false);
    }

    void OfferPurchase(string color, int price)
    {
        // Show Purchase Popup
        PurchasePanel.SetActive(true);

        // Change text accordingly to color and price
        PurchasePanel.transform.Find("Purchase Text").GetComponent<TMP_Text>().text = "Do you want to unlock " + color + " color for " + price.ToString() + " coins?";


        // Reset Listeners to avoid charging again
        PurchasePanel.transform.Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();

        // Check if user has enough money to buy color
        if (PlayerPrefs.GetInt("coinCount") >= price) {
            // Change button text if user have enough coins to buy color
            PurchasePanel.transform.Find("Button").Find("Unlock").GetComponent<TMP_Text>().text = "Unlock";
            // Link method with current args to onClick on Purchase button
            PurchasePanel.transform.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { InitiatePurchase(color, price); });
        }
        else
        {
            // Change button text if user have no enough coins to buy color
            PurchasePanel.transform.Find("Button").Find("Unlock").GetComponent<TMP_Text>().text = "Not Enough Coins";
        }
    }

    private void CheckUnlockedColors()
    {
        foreach (CarColor carColor in CarColors)
        {
            // Green is unlocked by default
            if(carColor.name != "green")
            {
                // Check if unlocked
                if(PlayerPrefs.GetInt(carColor.name + "Unlocked") == 1)
                {
                    // Capitalize color (e.g red => Red) and find Lock GameObject in hierarchy to hide it
                    Transform LockPanel = ColorsPanel.transform.Find(char.ToUpper(carColor.name[0]) + carColor.name[1..]).Find("Lock");

                    // Hide Lock
                    LockPanel.gameObject.SetActive(false);
                }
            }
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("greenUnlocked"))
        {
            // Set Green To Unlocked by default
            PlayerPrefs.SetInt("greenUnlocked", 1);
        }

        // Set default Color (green) if null
        if (!PlayerPrefs.HasKey("carColor"))
        {
            PlayerPrefs.SetString("carColor", "green");
        }

        // Only check\ color set if it's a garage scene (0)
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Check unlocked save
            CheckUnlockedColors();

            // // Set Color to saved from start
            // Iterate through every LOD in Car GameObjects array
            foreach (GameObject carPaint in CarPaint)
            {
                // Create a new Material Property Block
                var block = new MaterialPropertyBlock();

                // Set URP Base Color to needed color
                block.SetColor("_BaseColor", CarColors.Find(e => e.name == PlayerPrefs.GetString("carColor")).color);

                // Set Property Block to our Car Mesh Renderer
                carPaint.GetComponent<Renderer>().SetPropertyBlock(block);
            }
        }
    }

    public void InitiateChangeColor(string color)
    {
        // Only trigger animation change if the animator state is Idle (the color is not changing right now) to avoid overlapping.
        if (carAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {

            // Find CarColor object in color array by name
            CarColor carColor = CarColors.Find(e => e.name == color);

            if (PlayerPrefs.GetInt(color + "Unlocked") == 1)
            {
                // Trigger Car Color Switch Animation
                carAnimator.SetTrigger("SwitchColor");

                // Play Color Change sound
                carAudioSource.PlayDelayed(animationDelay - 0.25f);

                // If unlocked just change the color
                ChangeColor(carColor);
            }
            else
            {
                // If not unlocked offer to unlock
                OfferPurchase(carColor.name, carColor.price);
            }
        }
    }
}
