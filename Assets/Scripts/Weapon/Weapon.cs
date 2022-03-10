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
    private string nameOfSaveActive;
    private string nameOfSaveCost;
    private string nameOfSaveStrength;
    private string nameOfSavePrice;
    private string nameOfSaveDamage;

    void Start()
    {
        nameOfSaveActive = $"isActive{gameObject.name}";
        nameOfSaveCost = $"cost{gameObject.name}";
        nameOfSaveStrength = $"strenght{gameObject.name}";
        nameOfSavePrice = $"price{gameObject.name}";
        nameOfSaveDamage = $"Damage{gameObject.name}";

        if (PlayerPrefs.HasKey(nameOfSaveActive))
            isActive = PlayerPrefs.GetInt(nameOfSaveActive) == 1 ? true : false;
        else
            isActive = false;

        if (PlayerPrefs.HasKey(nameOfSaveCost))
            _cost = PlayerPrefs.GetInt(nameOfSaveCost);

        if (PlayerPrefs.HasKey(nameOfSaveStrength))
            _strenght = PlayerPrefs.GetInt(nameOfSaveStrength);

        if (PlayerPrefs.HasKey(nameOfSavePrice))
            _price = PlayerPrefs.GetInt(nameOfSavePrice);
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
        PlayerPrefs.SetInt(nameOfSaveActive, isActive ? 1 : 0);
    }


    public Sprite GetSprite() { return _sprite.sprite; }
    public string GetName() { return _name; }
    public int GetCost() { return _cost; }
    public int GetStrenght() { return _strenght; }
    public int GetPrice() { return _price; }
    public float GetDamageKoeficient() { return _damageKoeficient; }
    public bool GetActive() { return isActive; }

    public void Stand()
    {
        EventOnStand.Invoke();
    }

    [ContextMenu("Turn off")]
    public void TurnOff()
    {
        isActive = false;
        PlayerPrefs.SetInt(nameOfSaveActive, isActive ? 1 : 0);
    }

    public void SetCost(string value)
    {
        int.TryParse(value, out _cost);
        PlayerPrefs.SetInt(nameOfSaveCost, _cost);
        PlayerPrefs.Save();
    }
    public void SetStrenght(string value)
    {
        int.TryParse(value, out _strenght);
        PlayerPrefs.SetInt(nameOfSaveStrength, _strenght);
        PlayerPrefs.Save();
    }
    public void SetPrice(string value)
    {
        int.TryParse(value, out _price);
        PlayerPrefs.SetInt(nameOfSavePrice, _price);
        PlayerPrefs.Save();
    }
    public void SetDamageKoeficient(string value)
    {
        float.TryParse(value, out _damageKoeficient);
        PlayerPrefs.SetFloat(nameOfSaveDamage, _damageKoeficient);
        PlayerPrefs.Save();
    }
    public void SetActive(bool value)
    {
        isActive = value;
        PlayerPrefs.SetInt(nameOfSaveActive, isActive ? 1 : 0);
        PlayerPrefs.Save();
    }
}
