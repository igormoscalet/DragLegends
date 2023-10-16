
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

internal class Addressable_Spawner : MonoBehaviour
{
    // Label strings to load
    public List<string> keys = new List<string>() { "AICar", "PlayerCar" };

    // Operation handle used to load and release assets
    AsyncOperationHandle<IList<GameObject>> loadHandle;

    [System.Serializable]
    public class TransformByTag
    {
        public string tag;
        public float x, y, z;
    }

    public TransformByTag[] transformsByTag;

    public UnityEvent Ready;

    public GameObject Managers;

    public GearHelper GearHelper;

    public SpeedometerManager SpeedometerManager;

    public UpgradeColor ColorManager;

    public bool Done = false;

    public float carsRotation = 180f;


    // Load Addressables by Label
    public IEnumerator LoadAssets()
    {
        loadHandle = Addressables.LoadAssetsAsync<GameObject>(
            keys,
            addressable =>
            {
                Vector3 currentAssetPosition = new Vector3(0f,0f,0f);
                foreach(TransformByTag t in transformsByTag)
                {
                    if(t.tag == addressable.tag)
                    {
                        // gets pre-set transform position from struct to spawn cars in the right place
                        currentAssetPosition = new Vector3(t.x, t.y, t.z);
                    }
                }
                //Gets called for every loaded asset
                Instantiate<GameObject>(addressable,
                    currentAssetPosition,
                    // Rotate cars in needed direction depending on the scene
                    new Quaternion(0f, carsRotation, 0f, 1),
                    transform);
            }, Addressables.MergeMode.Union, // How to combine multiple labels 
            false); // Whether to fail and release if any asset fails to load

        yield return loadHandle;

        Ready.Invoke();
    }

    void Start()
    {
        Ready.AddListener(OnAssetsReady);
        StartCoroutine(LoadAssets());
    }

    private void OnAssetsReady()
    {
        Done = true;

        Managers.SetActive(true);
    }

    private void OnDestroy()
    {
        Addressables.Release(loadHandle);
        // Release all the loaded assets associated with loadHandle
        // Note that if you do not make loaded addressables a child of this object,
        // then you will need to devise another way of releasing the handle when
        // all the individual addressables are destroyed.
    }

    public void Update()
    {
        
    }
}