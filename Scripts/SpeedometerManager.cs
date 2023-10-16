using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedometerManager : MonoBehaviour
{
    public float startPosition = 210;
    public float endPosition = -30;
    public GameObject needle;
    public TMP_Text KPH;
    public TMP_Text Gear;

    public DragController carController = new DragController();
    public bool initialized = false;


    void FixedUpdate()
    {
        if (initialized)
        {
            float desiredPosition = startPosition - endPosition;
            float temp = carController.engineRPM / 10000;
            needle.transform.eulerAngles = new Vector3(0, 0, (startPosition - temp * desiredPosition));

            KPH.text = Mathf.FloorToInt(carController.KPH).ToString();
            Gear.text = carController.vertical >= 0 ? carController.gearNum.ToString() : "R";
        }
    }


}
