using UnityEngine;

[System.Serializable]
public class LevelData : Data
{
    [HideInInspector] public int index;
    public bool isSaved = false;
    [HideInInspector] public SerializableDictionary<string, ObjectDataWrapper> objectDataWrapper = new SerializableDictionary<string, ObjectDataWrapper>();
}
