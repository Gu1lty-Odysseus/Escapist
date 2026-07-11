using System;
using UnityEngine;

namespace Escapist.Audio
{
    [Serializable]
    public class AudioTriggerKey
    {
        [Tooltip("The unique event key identifier, e.g., 'PuzzleSolved'")]
        [SerializeField] private string eventName;
        
        [SerializeField] private AudioClip clip;
        [Range(0f, 1f)] [SerializeField] private float volume = 1f;

        public string EventName => eventName;
        public AudioClip Clip => clip;
        public float Volume => volume;
    }
}