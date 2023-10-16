using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnerBridge : MonoBehaviour
{
    public DistanceManager distanceManager;
    public GameObject focusPoint;
    public GameObject[] CarPaint;
    // Start is called before the first frame update
    void Start()
    {
        // distance manager set
        distanceManager = GameObject.Find("DistanceManager").GetComponent<DistanceManager>();

        // Camera focus set
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraOrbitFollow>().target = focusPoint.transform;


        foreach (GameObject carPaint in CarPaint)
        {
            // Create a new Material Property Block
            var block = new MaterialPropertyBlock();

            // Set URP Base Color to needed color
            block.SetColor("_BaseColor", GameObject.Find("ColorManager").GetComponent<UpgradeColor>().CarColors.Find(e => e.name == PlayerPrefs.GetString("carColor")).color);

            // Set Property Block to our Car Mesh Renderer
            carPaint.GetComponent<Renderer>().SetPropertyBlock(block);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // translate player position for distancemanager
        distanceManager.playerPosition = transform.position;


    }
}
