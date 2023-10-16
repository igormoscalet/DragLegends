using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseLights : MonoBehaviour
{
    public DragController car;
    private Renderer m_Renderer;


    private void Start()
    {
        m_Renderer = GetComponent<Renderer>();
    }


    private void Update()
    {
        // enable the Renderer when the car is braking, disable it otherwise.
        m_Renderer.enabled = car.vertical < 0;
    }
}
