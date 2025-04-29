using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialog : MonoBehaviour
{
    protected Canvas canvas;
    protected List<MonoBehaviour> scripts = new List<MonoBehaviour>();
    [HideInInspector] public UnityEvent<bool> showEvent = new UnityEvent<bool> ();

    protected virtual void Awake() {
        canvas = GetComponent<Canvas>();
        foreach (var script in GetComponentsInChildren<MonoBehaviour>()) {
            scripts.Add(script);
        }
    }

    public virtual void Init(DialogParamater paramater) { }

    public virtual void Open() {
        showEvent.Invoke(true);
        canvas.enabled = true;
        foreach (var script in scripts) {
            script.enabled = true;
        }
    }

    public virtual void Close() {
        showEvent.Invoke(false);
        canvas.enabled = false;
        foreach (var script in scripts) {
            script.enabled = false;
        }
    }
}
