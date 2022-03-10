using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PlaceType
{
    NoLimit,
    Walls,
    Roof,
    Floor,
    NotPlaceble
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private int _cost;
    [SerializeField] private float _damageKoeficient = 1f;
    [SerializeField] private PlaceType _placeType;
    [SerializeField] private Vector2Int _size = new Vector2Int(1, 1);
    [SerializeField] protected int _strenght = 1;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] private int _price;
    [SerializeField] private UnityEvent EventOnStart;
    [SerializeField] private UnityEvent EventOnStand;
    [SerializeField] private UnityEvent EventOnDestroy;
    public Collider2D Collider;
    private bool isActive;
    private string nameOfSave;

    void Start()
    {
        nameOfSave = $"isActive{gameObject.name}";
        if (PlayerPrefs.HasKey(nameOfSave))
            isActive = PlayerPrefs.GetFloat(nameOfSave) == 1 ? true : false;
        else
            isActive = false;
        EventOnStart.Invoke();
    }

    public float GetKoeficient() { return _damageKoeficient; }
    public PlaceType GetPlaceType() { return _placeType; }
    public Vector2Int GetSize() { return _size; }

    protected void ClearDictionary()
    {
        for (int i = 0; i < _size.x; i++)
        {
            for (int j = 0; j < _size.y; j++)
            {
                Vector2Int cell = new Vector2Int((int)transform.position.x + i, (int)transform.position.x + j);
                if (GameManager.Instance.WeaponDictionary.ContainsKey(cell))
                    GameManager.Instance.WeaponDictionary.Remove(cell);
            }
        }
    }
    void OnDestroy()
    {
        EventOnDestroy.Invoke();
        ClearDictionary();
    }

    public virtual void DivStrength()
    {
        _strenght--;
        if (_strenght <= 0)
            Destroy(gameObject);
    }

    public void Buy()
    {
        isActive = true;
        PlayerPrefs.SetInt(nameOfSave, isActive ? 1 : 0);
    }


    public Sprite GetSprite() { return _sprite.sprite; }
    public string GetName() { return _name; }
    public int GetCost() { return _cost; }
    public int GetStrenght() { return _strenght; }
    public int GetPrice() { return _price; }
    public bool GetActive() { return isActive; }

    public void Stand()
    {
        EventOnStand.Invoke();
    }

    [ContextMenu("Turn off")]
    public void TurnOff()
    {
        isActive = false;
        PlayerPrefs.SetInt(nameOfSave, isActive ? 1 : 0);
    }
}
