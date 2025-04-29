using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pharmaceutical", menuName = "Pharmaceutical/Pharmaceutical")]
public class Pharmaceutical : Effect
{
    public int index;
    public Sprite sprite;
    public int price;
    public float effectTime;

    public bool isActive;
    public int count;
    public bool hasAttention;
    public List<float> timeRemainingList;

    private void OnValidate() {
        if (!isActive) {
            count = 0;
            for (int i = 0; i < timeRemainingList.Count; i++) {
                timeRemainingList[i] = 0;
            }
        }
    }
}
