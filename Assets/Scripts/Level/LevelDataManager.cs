using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDataManager : MonoBehaviour
{
    private Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
    private Dictionary<string, Animal> animals = new Dictionary<string, Animal>();

    private void Awake() {
        GameManager.Instance.SetLevelDataManager(this);
    }

    public Dictionary<string, Tile> GetTileObjects() {
        if (tiles.Count == 0) {
            Tile[] t = FindObjectsByType<Tile>(FindObjectsSortMode.None);
            foreach (Tile tile in t) {
                if (tile != null) {
                    tiles.Add(tile.name, tile);
                }
            }
        }
        return tiles;
    }

    public Dictionary<string, Animal> GetAnimalObjects() {
        if (animals.Count == 0) {
            Animal[] a = FindObjectsByType<Animal>(FindObjectsSortMode.None);
            foreach (Animal animal in a) {
                if (animal != null) {
                    animals.Add(animal.name, animal);
                }
            }
        }
        return animals;
    }
}
