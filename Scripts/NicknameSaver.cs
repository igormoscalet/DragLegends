using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NicknameSaver : MonoBehaviour
{

    public GameObject usernamePopup;
    public GameObject welcomeIcon;
    public TMP_Text userInput;
    public TMP_Text userPlaceholder;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("username"))
        {
            usernamePopup.SetActive(true);
            welcomeIcon.SetActive(false);
        }
        else
        {
            userPlaceholder.text = PlayerPrefs.GetString("username");
        }
    }

    public void SaveUsername()
    {
        PlayerPrefs.SetString("username", userInput.text);
        welcomeIcon.SetActive(true);
        userPlaceholder.text = userInput.text;
    }
}
