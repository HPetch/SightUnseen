using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(GrenadeReceiver))]
public class GrenadeViewEditor : Editor
{
    private void OnSceneGUI()
    {
        GrenadeReceiver script = (GrenadeReceiver)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(script.transform.position, Vector3.up, Vector3.forward, 360, script.viewRadius);
        Vector3 ViewAngleA = script.DirFromAngle(-script.viewAngle / 2, false);
        Vector3 ViewAngleB = script.DirFromAngle(script.viewAngle / 2, false);

        Handles.DrawLine(script.transform.position, script.transform.position + ViewAngleA * script.viewRadius);
        Handles.DrawLine(script.transform.position, script.transform.position + ViewAngleB * script.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in script.visibleTargets)
        {
            Handles.DrawLine(script.transform.position, visibleTarget.position);
        }
    }
}
