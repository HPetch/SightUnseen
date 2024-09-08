using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePrefabMeshRenderers : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabsToSearch;
    [SerializeField] private int targetLayer = 0; //Default by... default
    //[SerializeField] private bool hideImmediately = true;
    private List<Renderer> renderers;

    void Awake()
    {
        renderers = new List<Renderer>();
        //Find every mesh renderer in the relevant prefabs' children and if they are equal to the right layer, add to the array
        foreach (GameObject i in prefabsToSearch)
        {
            Renderer[] prefabRenderers = i.GetComponentsInChildren<Renderer>();
            foreach (Renderer y in prefabRenderers)
            {
                if (y.gameObject.layer == targetLayer)
                {
                    Debug.Log("WOO");
                    renderers.Add(y);
                    //if (hideImmediately) y.enabled = false;
                }
            } 
        }
        HideRenderers();
    }

    public void HideRenderers()
    {
        foreach (Renderer item in renderers)
        {
            item.enabled = false;
        }
    }

    public void ShowRenderers()
    {
        foreach (Renderer item in renderers)
        {
            item.enabled = true;
        }
    }
}
