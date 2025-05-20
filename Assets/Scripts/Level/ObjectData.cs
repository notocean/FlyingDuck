using System;
using UnityEngine;

[SerializeField]
public abstract class ObjectData {
    public abstract ObjectData Clone();
}

[Serializable]
public class ObjectDataWrapper {
    [SerializeReference]
    public ObjectData data;

    public ObjectDataWrapper(ObjectData data) {
        this.data = data;
    }

    public ObjectDataWrapper Clone() {
        return new ObjectDataWrapper(data.Clone());
    }
}