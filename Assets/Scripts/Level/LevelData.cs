using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData : Data
{
    [HideInInspector] public int index;
    public bool isSaved = false;
    [HideInInspector] public SerializableDictionary<string, ObjectDataWrapper> objectDataWrapper = new SerializableDictionary<string, ObjectDataWrapper>();

    public LevelData Clone() {
        LevelData clone = new LevelData();
        clone.index = index;
        clone.isSaved = isSaved;

        foreach (KeyValuePair<string, ObjectDataWrapper> keyValuePair in objectDataWrapper) {
            clone.objectDataWrapper.Add(keyValuePair.Key, keyValuePair.Value.Clone());
        }

        return clone;
    }
}
