using System.IO;
using UnityEngine;

namespace Escapist.Persistence
{
    public class SaveSystem : MonoBehaviour
    {
        public static SaveSystem Instance { get; private set; }

        private string saveFilePath;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Path resolves securely based on individual client operating platform targets
            saveFilePath = Path.Combine(Application.persistentDataPath, "escapist_savegame.json");
        }

        [ContextMenu("Save Game Game State")]
        public void SaveGame()
        {
            SaveData currentSavePayload = new SaveData();

            // Locate all interface implementations currently residing in scene hierarchies
            var saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            
            foreach (var mono in saveables)
            {
                if (mono is ISaveable saveable)
                {
                    saveable.CaptureState(currentSavePayload);
                }
            }

            // Convert state configurations cleanly to highly human-readable JSON formats
            string jsonOutput = JsonUtility.ToJson(currentSavePayload, true);
            
            try
            {
                File.WriteAllText(saveFilePath, jsonOutput);
                Debug.Log($"[SaveSystem] Game state archived successfully to: {saveFilePath}");
            }
            catch (IOException ex)
            {
                Debug.LogError($"[SaveSystem] Failed to write out to disk array metrics: {ex.Message}");
            }
        }

        [ContextMenu("Load Game State")]
        public void LoadGame()
        {
            if (!File.Exists(saveFilePath))
            {
                Debug.LogWarning("[SaveSystem] Load operation aborted. No existing file signature identified on target pathing.");
                return;
            }

            try
            {
                string jsonInput = File.ReadAllText(saveFilePath);
                SaveData activePayload = JsonUtility.FromJson<SaveData>(jsonInput);

                var saveables = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
                
                foreach (var mono in saveables)
                {
                    if (mono is ISaveable saveable)
                    {
                        saveable.RestoreState(activePayload);
                    }
                }
                Debug.Log("[SaveSystem] Active state definitions cleanly loaded and distributed.");
            }
            catch (IOException ex)
            {
                Debug.LogError($"[SaveSystem] Failed parsing active file streams: {ex.Message}");
            }
        }
    }
}