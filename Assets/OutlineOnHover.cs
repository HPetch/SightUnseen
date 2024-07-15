using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core;

public class OutlineOnHover : MonoBehaviour
{
    private Outline hoverObjectOutline;

    public void enableOutline(HVRGrabbable hovered)
    {
        if (hoverObjectOutline != null)
            hoverObjectOutline.enabled = true;
    }

    public void disableOutline(HVRGrabbable hovered)
    {
        if (hoverObjectOutline != null)
            hoverObjectOutline.enabled = false;
    }

}
