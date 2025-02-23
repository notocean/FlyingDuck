using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HairOutfit", menuName = "Duck/HairOutfit")]
public class HairOutfit : ScriptableObject
{
    public int index;
    public Sprite sprite;
    public Sprite spriteUI;
    public bool isActive;
}
