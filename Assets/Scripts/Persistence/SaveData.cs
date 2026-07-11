using System;
using System.Collections.Generic;

namespace Escapist.Persistence
{
    [Serializable]
    public class SaveData
    {
        // --- Player Position & Rotation Tracking ---
        public SerializableVector3 playerPosition;
        public SerializableVector3 playerRotation;

        // --- Inventory Subsystem States ---
        public List<SerializableInventorySlot> inventorySlots = new List<SerializableInventorySlot>();

        // --- Puzzle Progress Tracking ---
        public List<SerializablePuzzleState> puzzleStates = new List<SerializablePuzzleState>();

        // Pure C# Helper Structs enabling JsonUtility compliance
        [Serializable]
        public struct SerializableVector3
        {
            public float x, y, z;
            public SerializableVector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
            public static implicit operator UnityEngine.Vector3(SerializableVector3 s) => new UnityEngine.Vector3(s.x, s.y, s.z);
            public static implicit operator SerializableVector3(UnityEngine.Vector3 v) => new SerializableVector3(v.x, v.y, v.z);
        }

        [Serializable]
        public struct SerializableInventorySlot
        {
            public string itemId;
            public int quantity;
        }

        [Serializable]
        public struct SerializablePuzzleState
        {
            public string puzzleId;
            public bool isCompleted;
        }
    }
}