using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapHandler : MonoBehaviour
{
    public GameObject[] maps;

    public int currentMap = 0;

    public Animator garageDoor;

    public GameObject mapsCanvas;

    public GameObject[] mapsExterior;

    public Material daySky;


    // Simple slider logic
    public void nextMap()
    {
        currentMap++;
        if(currentMap > maps.Length - 1)
        {
            currentMap = 0;
        }

        // render after current map index is calculated
        renderMap();
    }

    public void previousMap()
    {
        currentMap--;
        if(currentMap == -1)
        {
            currentMap = maps.Length - 1;
        }

        renderMap();
    }

    private void renderMap()
    {
        // Show only current map and hide all others
        for (int i = 0; i < maps.Length; i++)
        {
            maps[i].SetActive(false);

            if (i == currentMap)
            {
                maps[i].SetActive(true);
            }
        }
    }

    public void LoadMap()
    {
        // Render exterior according to scene before door opens
        foreach(GameObject exterior in mapsExterior)
        {
            exterior.SetActive(false);
        }

        // Show only exterior for corresponding scene
        mapsExterior[currentMap].SetActive(true);

        // If its scene 2 - turn on day sky [can be scaled // also ^ can be optimized with Addressables]
        RenderSettings.skybox = daySky;

        // Hide maps menu
        mapsCanvas.SetActive(false);

        // Play garage door opening animation before loading new scene
        garageDoor.SetTrigger("OpenDoor");

        // Play door open sound
        garageDoor.GetComponent<AudioSource>().Play();

        Invoke("SceneLoad", 3f);
    }

    private void SceneLoad()
    {
        // This is only gonna work if the scenes will be set in the same order as in map list in Canvas
        SceneManager.LoadScene(currentMap + 1);
    }
}
