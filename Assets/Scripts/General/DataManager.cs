using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataManager : ScriptableObject {
    public abstract void Save();
    public abstract void Load();
}
