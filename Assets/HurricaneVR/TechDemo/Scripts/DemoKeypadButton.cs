using HurricaneVR.Framework.Components;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HurricaneVR.TechDemo.Scripts
{
    public class DemoKeypadButton : HVRPhysicsButton
    {
        public List<AudioClip> buttonSounds;
        public AudioSource source;

        public char Key;
        public TextMeshPro TextMeshPro;

        protected override void Awake()
        {
            ConnectedBody = transform.parent.GetComponentInParent<Rigidbody>();
            base.Awake();
        }

        public void PlaySound()
        {
            AudioClip audioToPlay;
            audioToPlay = buttonSounds[Random.Range(0, buttonSounds.Count)];
            source.clip = audioToPlay;
            source.Play();
        }
    }
}