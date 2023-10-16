using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GearHelper : MonoBehaviour
{
    public TMP_Text upperArrow;
    public DragController carController = new DragController();

    public bool initialized = false;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (initialized)
        {
            float engineRPM = carController.engineRPM;

            if (engineRPM >= 4300 && engineRPM <= 5000)
            {
                upperArrow.color = Color.green;
            }
            else if (engineRPM > 5000)
            {
                upperArrow.color = Color.red;
            }
            else if (engineRPM < 4300 && engineRPM > 3900)
            {
                upperArrow.color = new Color(0f, 175f, 255f);
            }
            else
            {
                upperArrow.color = Color.white;
            }
        }
    }
}

