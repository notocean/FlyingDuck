using UnityEngine;

public abstract class DataManager : ScriptableObject {
    public abstract void Save();
    public abstract void Load();
}

[System.Serializable]
public class Data { }
