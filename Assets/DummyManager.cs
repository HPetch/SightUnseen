using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DummyManager : MonoBehaviour
{
    [SerializeField] private GameObject dummyPlayerPrefab;
    [SerializeField] private bool spawnAtEditorCamera = false;
    private GameObject dummy; //Current Dummy Player
    private Vector3 spawnLocation;

    // Start is called before the first frame update
    void Awake()
    {
        //If a VR headset isn't connected, add in DummyPlayer and disable HurricaneVR logic
        if (!XRSettings.enabled)
        {
            Debug.LogWarning("XR is disabled/not detected. Disabling HVR objects.");
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("HVR Setting"))
            {
                i.SetActive(false);
            }
            foreach (GameObject i in GameObject.FindGameObjectsWithTag("HVR Player"))
            {
                //Capture spawn location and rotation for where to initialise
                spawnLocation = i.transform.position;
                i.SetActive(false);
            }

            //Add dummy player controllable with keyboard and mouse
            Debug.Log("All HVR objects disabled, adding DummyPlayer prefab.");
            if (!spawnAtEditorCamera) Instantiate(dummyPlayerPrefab, spawnLocation, Quaternion.identity);
            else
            {
                //If editor camera is picked, get the editor camera's position
                dummy = Instantiate(dummyPlayerPrefab, GetComponent<EditorCameraPosition>().getEditorPos(), Quaternion.identity);
            }
        }
    }
}
