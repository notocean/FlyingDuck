using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pharmaceutical : ScriptableObject
{
    public int index;
    public Sprite sprite;
    public int price;
    public string infor;
    public float effectTime;

    public bool isActive;
    public int count;
    public float timeRemaining;

    public abstract void ApplyEffect(GameObject target);
    public abstract void EndEffect(GameObject target);
}
