using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlaceType
{
    NoLimit,
    Walls,
    Roof,
    Floor
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private int _price;
    [SerializeField] private float _borderVelocity = 10f;
    [SerializeField] private float _damageKoeficient = 1f;
    [SerializeField] private PlaceType _placeType;

    public float GetBorderVelocity() { return _borderVelocity; }
    public float GetKoeficient() { return _damageKoeficient; }
    public int GetPrice() { return _price; }
    public PlaceType GetPlaceType() { return _placeType; }
}
