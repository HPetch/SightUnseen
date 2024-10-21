using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHandMaterialFade : MonoBehaviour
{
    public Renderer myModel;
    [SerializeField][Range(0, 10)] private float maxRange = 3;
    [SerializeField][Range(0, 10)] private float minRange = 1;
    public GameObject eye;
    private float presetAlpha;
    // Start is called before the first frame update
    void Start()
    {
        presetAlpha = myModel.material.color.a;
        //eye = GameObject.Find("Detached Eyeball 2");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, eye.transform.position);
        //If player is beyond max range or beyond min range, we don't need to update as they are at their topmost values.
        //If we have closed in past the max range we need to start adjusting the alpha
        if ((distance < maxRange) && (distance > minRange))
        {
            //Get a percentage of how far along we are
            float difference = maxRange - distance;
            float percentage = (difference / (maxRange - minRange));

            //Color color = myModel.material.color;
            //color.a = percentage;
            //myModel.material.color = color;

            //Apply that percentage into a value between 0 and 255 and apply that to the alpha channel
            myModel.material.color = new Color(myModel.material.color.r, myModel.material.color.g, myModel.material.color.b, Mathf.Lerp(0, presetAlpha, percentage));
        }
    }
}
