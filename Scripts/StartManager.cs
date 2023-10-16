using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartManager : MonoBehaviour
{

    public TMP_Text StartText;
    public float outTime = 3f;
    public GameObject[] StartCollider;

    public bool raceStarted = false;
    public float raceTime = 0f;


    // Update is called once per frame
    void Update()
    {
        outTime -= Time.deltaTime;
        StartText.text = Mathf.CeilToInt(outTime).ToString();

        if(outTime < 0)
        {
            StartText.gameObject.SetActive(false);
            foreach (GameObject startCollider in StartCollider)
            {
                startCollider.SetActive(false);
            }
            raceStarted = true;
        }

        if (raceStarted)
        {
            raceTime += Time.deltaTime;
        }
    }
}
