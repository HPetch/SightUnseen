using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INab.WorldScanFX.Builtin
{
    [ImageEffectAllowedInSceneView]
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    public class ScanFX : ScanFXBase
    {
        [SerializeField] private Camera currentCamera;
        
        private void OnEnable()
        {
            currentCamera = GetComponent<Camera>();
            currentCamera.depthTextureMode = DepthTextureMode.DepthNormals;
        }

        [ImageEffectOpaque]
        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            if (scanOrigin == null || scanMaterial == null)
            {
                Graphics.Blit(source, destination);
                Debug.LogWarning("Scan Origin or Scan Material is not assigned");
                return;
            }

            scanMaterial.SetMatrix("_InverseView", Camera.current.cameraToWorldMatrix);

            Graphics.Blit(source, destination, scanMaterial);
        }

    }
}