using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSprite", menuName = "Level/LevelSprite")]
public class LevelSprite : ScriptableObject {
    public Sprite topSprite;
    public Sprite centerSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
}
