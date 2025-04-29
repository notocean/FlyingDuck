using System;
using UnityEngine;

[SerializeField]
public class ObjectData { }

[Serializable]
public class ObjectDataWrapper {
    [SerializeReference]
    public ObjectData data;

    public ObjectDataWrapper(ObjectData data) {
        this.data = data;
    }
}