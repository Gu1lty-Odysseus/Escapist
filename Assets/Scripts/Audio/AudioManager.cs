using System.Collections.Generic;
using UnityEngine;

namespace Escapist.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Event Asset Arrays")]
        [SerializeField] private List<AudioTriggerKey> soundRegistry = new List<AudioTriggerKey>();

        [Header("Performance Optimization Pools")]
        [SerializeField] private int initialPoolSize = 6;

        private Dictionary<string, AudioTriggerKey> registryMap = new Dictionary<string, AudioTriggerKey>();
        private List<AudioSource> audioSourcePool = new List<AudioSource>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeRegistry();
            InitializeSourcePool();
        }

        private void InitializeRegistry()
        {
            foreach (var mapping in soundRegistry)
            {
                if (!string.IsNullOrEmpty(mapping.EventName) && !registryMap.ContainsKey(mapping.EventName))
                {
                    registryMap.Add(mapping.EventName, mapping);
                }
            }
        }

        private void InitializeSourcePool()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewAudioSourceInstance();
            }
        }

        private AudioSource CreateNewAudioSourceInstance()
        {
            GameObject sourceObj = new GameObject($"AudioSource_Channel_{audioSourcePool.Count}");
            sourceObj.transform.SetParent(transform);
            
            AudioSource source = sourceObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            
            audioSourcePool.Add(source);
            return source;
        }

        /// <summary>
        /// Globally dispatches a programmatic sound call across available hardware channels.
        /// Decoupled from physical dependencies via simple string key signatures.
        /// </summary>
        public void PlaySound(string eventName)
        {
            if (!registryMap.TryGetValue(eventName, out AudioTriggerKey soundKey))
            {
                Debug.LogWarning($"[AudioManager] Dispatched request for unregistered audio event key: '{eventName}'");
                return;
            }

            AudioSource availableSource = GetAvailableSourceFromPool();
            
            availableSource.clip = soundKey.Clip;
            availableSource.volume = soundKey.Volume;
            availableSource.Play();
        }

        private AudioSource GetAvailableSourceFromPool()
        {
            foreach (var source in audioSourcePool)
            {
                if (!source.isPlaying) return source;
            }

            // Fallback expansion constraint protection block to prevent voice drop conditions
            return CreateNewAudioSourceInstance();
        }
    }
}