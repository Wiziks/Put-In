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
    [SerializeField] private string _nameUkr;
    [SerializeField] private string _nameRus;
    [SerializeField] private string _nameEng;
    [SerializeField] private int _cost;
    [SerializeField] private float _damageKoeficient = 1f;
    [SerializeField] private PlaceType _placeType;
    [SerializeField] private Vector2Int _size = new Vector2Int(1, 1);
    [SerializeField] protected int _strenght = 1;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] private int _scoreToUnlock;
    [SerializeField] private UnityEvent EventOnStart;
    [SerializeField] private UnityEvent EventOnStand;
    [SerializeField] private UnityEvent EventOnDestroy;
    public Collider2D Collider;
    private bool isActive;
    private string nameOfSaveActive;
    private string nameOfSaveCost;
    private string nameOfSaveStrength;
    private string nameOfSaveScore;
    private string nameOfSaveDamage;

    void Awake()
    {
        nameOfSaveActive = $"isActive{gameObject.name}";

        if (PlayerPrefs.HasKey(nameOfSaveActive))
            isActive = PlayerPrefs.GetInt(nameOfSaveActive) == 1 ? true : false;
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
                Vector2 cell = new Vector2(transform.position.x + i, transform.position.y + j);
                if (GameManager.Instance.WeaponDictionary.ContainsKey(cell))
                    GameManager.Instance.WeaponDictionary.Remove(cell);
            }
        }
    }
    void OnDestroy()
    {
        AudioManager.Instance.PlaySoundBreake();
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
    public string GetName()
    {
        string name = "";
        if (Localization.Instance.GetLanguage() == Language.Ukrainian)
            name = _nameUkr;
        else if (Localization.Instance.GetLanguage() == Language.Russian)
            name = _nameRus;
        else if (Localization.Instance.GetLanguage() == Language.English)
            name = _nameEng;
        return name;
    }
    public int GetCost() { return _cost; }
    public int GetStrenght() { return _strenght; }
    public int GetUnlockScore() { return _scoreToUnlock; }
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
        if (SnapScrolling.Instance)
            SnapScrolling.Instance.RefreshAll();
    }
    public void SetStrenght(string value)
    {
        int.TryParse(value, out _strenght);
        PlayerPrefs.SetInt(nameOfSaveStrength, _strenght);
        PlayerPrefs.Save();
        if (SnapScrolling.Instance)
            SnapScrolling.Instance.RefreshAll();
    }
    public void SetUnlockScore(string value)
    {
        int.TryParse(value, out _scoreToUnlock);
        PlayerPrefs.SetInt(nameOfSaveScore, _scoreToUnlock);
        PlayerPrefs.Save();
        if (SnapScrolling.Instance)
            SnapScrolling.Instance.RefreshAll();
    }
    public void SetDamageKoeficient(string value)
    {
        float.TryParse(value, out _damageKoeficient);
        PlayerPrefs.SetFloat(nameOfSaveDamage, _damageKoeficient);
        PlayerPrefs.Save();
        if (SnapScrolling.Instance)
            SnapScrolling.Instance.RefreshAll();
    }
    public void SetActive(bool value)
    {
        isActive = value;
        PlayerPrefs.SetInt(nameOfSaveActive, isActive ? 1 : 0);
        PlayerPrefs.Save();
        if (SnapScrolling.Instance)
            SnapScrolling.Instance.RefreshAll();
    }

    public string GetPlaceTypeString()
    {
        string placeName = "";
        if (_placeType == PlaceType.Roof)
        {
            placeName = Localization.Instance.GetRightPhase(26);
        }
        else if (_placeType == PlaceType.Floor)
        {
            placeName = Localization.Instance.GetRightPhase(27);
        }
        else if (_placeType == PlaceType.Walls)
        {
            placeName = Localization.Instance.GetRightPhase(28);
        }
        else if (_placeType == PlaceType.NoLimit)
        {
            placeName = Localization.Instance.GetRightPhase(29);
        }
        else if (_placeType == PlaceType.NotPlaceble)
        {
            placeName = Localization.Instance.GetRightPhase(30);
        }
        return placeName;
    }
}
