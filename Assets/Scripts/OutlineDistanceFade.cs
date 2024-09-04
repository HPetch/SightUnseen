using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineDistanceFade : MonoBehaviour
{
    private Outline outline;
    private GameObject player;
    [SerializeField] [Range(0, 10)] private float maxRange = 3;
    [SerializeField] [Range(0, 10)] private float minRange = 1;
    private float presetAlpha;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        player = GameObject.Find("PlayerController");
        presetAlpha = outline.OutlineColor.a;
        outline.OutlineColor = new Color(outline.OutlineColor.r, outline.OutlineColor.g, outline.OutlineColor.b,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            //If player is beyond max range or beyond min range, we don't need to update as they are at their topmost values.
            //If we have closed in past the max range we need to start adjusting the alpha
            if ((distance < maxRange) && (distance > minRange))
            {
                //Get a percentage of how far along we are
                float difference = maxRange - distance;
                float percentage = (difference / (maxRange-minRange));

                //Apply that percentage into a value between 0 and 255 and apply that to the alpha channel
                outline.OutlineColor = new Color(outline.OutlineColor.r, outline.OutlineColor.g, outline.OutlineColor.b, Mathf.Lerp(0, presetAlpha, percentage));
            }
        }
    }
}
