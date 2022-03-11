using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resource : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinsText;
    public static int Coins { get; private set; }
    private static string nameOfSave = "ResourceCoins";
    public static Resource Instance;

    void Start()
    {
        Coins = 0;
        Instance = this;
        if (PlayerPrefs.HasKey(nameOfSave))
        {
            Coins = PlayerPrefs.GetInt(nameOfSave);
        }
        _coinsText.text = $"{Coins}";
    }

    public void ChangeValue(int value)
    {
        Coins += value;
        _coinsText.text = $"{Coins}";
        PlayerPrefs.SetInt(nameOfSave, Coins);
        PlayerPrefs.Save();
    }

    public bool TryBuy(int price)
    {
        if (Coins < price)
        {
            AudioManager.Instance.PlaySoundNEM();
            InformationText.Instance.Activate();
            return false;
        }
        Coins -= price;
        ChangeValue(-price);
        AudioManager.Instance.PlaySoundBuy();
        return true;
    }
}
