using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinishTrigger : MonoBehaviour
{

    public StartManager startManager;

    public GameObject WinScreen;
    public TMP_Text WinDescription;
    public GameObject LoseScreen;
    public TMP_Text LoseDescription;

    public float opponentFinish = 0f;
    public float playerFinish = 0f;

    private bool playerFinished = false;

    private bool opponentArrived = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !playerFinished)
        {
            // takes timestamp of racetime when player crosses the finish line
            playerFinish = startManager.raceTime;

            if (opponentArrived)
            {
                Lose();
            }
            else
            {
                Win();
            }

            // Checks if player is finished already next time to not update with wrong timestamp in case player goes
            // in trigger again while showing the finish screen (the player can still move)
            playerFinished = true;
        }
        else if(other.tag == "AI")
        {
            // takes timestamp of racetime when opponent crosses the finish line
            opponentFinish = startManager.raceTime;
            opponentArrived = true;
        }
    }

    void Win()
    {
        WinScreen.SetActive(true);
        WinDescription.text = "You beat your opponent in " + (Mathf.Round(playerFinish * 100f) / 100f).ToString() + " seconds";

        // check how much wins we have already
        int winsAlready = PlayerPrefs.GetInt("Wins", 0);

        // add one to our win count
        PlayerPrefs.SetInt("Wins", winsAlready++);
    }

    void Lose()
    {
        LoseScreen.SetActive(true);
        LoseDescription.text = "Your opponent beat you and arrived " + (Mathf.Round((playerFinish - opponentFinish) * 100f) / 100f).ToString() + " seconds ahead of you";

        // check how much losses we have already
        int lossesAlready = PlayerPrefs.GetInt("Losses", 0);

        // add one to your loss count
        PlayerPrefs.SetInt("Losses", lossesAlready++);
    }

}
