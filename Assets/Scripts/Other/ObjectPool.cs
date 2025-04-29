using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    [SerializeField] GameObject prefab;
    [SerializeField] int initCount;
    private Queue<GameObject> pool = new Queue<GameObject>();
    int index = 0;

    private void Awake() {
        ObjectPoolingManager.Instance.RegisterPool(name, this);
        InitPrefabs();
    }

    void InitPrefabs() {
        for (int i = 0; i < initCount; i++) {
            GameObject obj = Instantiate(prefab);
            IHidableObject hidableObject = obj.GetComponent<IHidableObject>();
            if (hidableObject != null) {
                hidableObject.SetVisible(false);
            }
            else obj.SetActive(false);
            obj.name = obj.name + $" {++index}";
            pool.Enqueue(obj);
        }
    }

    public GameObject GetObject() {
        if (pool.Count > 0) {
            GameObject obj = pool.Dequeue();
            IHidableObject hidableObject = obj.GetComponent<IHidableObject>();
            if (hidableObject != null) {
                hidableObject.SetVisible(true);
            }
            else obj.SetActive(true);
            return obj;
        }
        else {
            GameObject obj = Instantiate(prefab);
            obj.name = obj.name + $" {++index}";
            return obj;
        }
    }

    public void ReturnObject(GameObject obj) {
        IHidableObject hidableObject = obj.GetComponent<IHidableObject>();
        if (hidableObject != null) {
            hidableObject.SetVisible(false);
        }
        else obj.SetActive(false);
        pool.Enqueue(obj);
    }
}

