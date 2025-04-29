using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolingManager : MonoBehaviour
{
    private static ObjectPoolingManager _instance;
    public static ObjectPoolingManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ObjectPoolingManager>();
                if (_instance == null) {
                    _instance = new GameObject("ObjectPoolingManager").AddComponent<ObjectPoolingManager>();
                }
            }

            return _instance;
        }
    }

    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    private void Awake() {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else _instance = this;
    }

    public void RegisterPool(string key, ObjectPool pool) {
        if (!pools.ContainsKey(key)) pools.Add(key, pool);
    }

    public ObjectPool GetPool(string key) {
        if (pools.ContainsKey(key)) return pools[key];
        return null;
    }

    public GameObject GetObject(string key) {
        if (pools.ContainsKey(key))
            return pools[key].GetObject();
        return null;
    }
}
