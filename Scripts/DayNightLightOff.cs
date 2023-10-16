using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightLightOff : MonoBehaviour
{
    public GameObject[] Lights;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("DayScene"))
        {
            foreach (GameObject light in Lights)
            {
                light.SetActive(false);
            }
        }
    }
}
