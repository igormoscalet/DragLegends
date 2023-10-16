using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DistanceManager : MonoBehaviour
{
    public TMP_Text distanceText;
    public float distanceToFinish = 0f;
    public GameObject finishBox;

    public Vector3 playerPosition;

    // Update is called once per frame
    void Update()
    {
        distanceToFinish = Vector3.Distance(playerPosition, finishBox.transform.position);
        distanceText.text = Mathf.FloorToInt(distanceToFinish).ToString() + "m";
    }
}
