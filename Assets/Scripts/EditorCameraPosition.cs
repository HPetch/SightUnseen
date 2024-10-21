using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

[ExecuteInEditMode]
public class EditorCameraPosition : MonoBehaviour
{
    public Vector3 editorCamPos; //This variable needs to be public, if it is private, it resets on play?

    //Every time the camera moves or something is changed in the editor scene, this event triggers
    private void OnRenderObject()
    {
        //Every time the camera updates in the editor, update the Vector3.
        /*if (Application.isEditor && !Application.isPlaying)
        {
            Camera sceneCamera = SceneView.lastActiveSceneView.camera;
            editorCamPos = sceneCamera.transform.position;
        }*/
    }

    public Vector3 getEditorPos()
    {
        return editorCamPos;
    }
}
