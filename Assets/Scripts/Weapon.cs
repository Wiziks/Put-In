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
    [SerializeField] private Vector2Int _size = new Vector2Int(1, 1);
    public Collider2D Collider;

    public float GetBorderVelocity() { return _borderVelocity; }
    public float GetKoeficient() { return _damageKoeficient; }
    public int GetPrice() { return _price; }
    public PlaceType GetPlaceType() { return _placeType; }
    public Vector2Int GetSize() { return _size; }

    protected void ClearDictionary()
    {
        for (int i = 0; i < _size.x; i++)
        {
            for (int j = 0; j < _size.y; j++)
            {
                Vector2Int cell = new Vector2Int((int)transform.position.x + i, (int)transform.position.x + j);
                if(GameManager.WeaponDictionary.ContainsKey(cell))
                    GameManager.WeaponDictionary.Remove(cell);
            }
        }
    }
    void OnDestroy()
    {
        ClearDictionary();
    }
}
