using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    public abstract void SetAnimalData(AnimalData animalData);
    public abstract AnimalData GetAnimalData();
    public abstract int Collected();
}
