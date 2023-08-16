using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resource : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _coinsText;
    [SerializeField] private TextMeshProUGUI _maxScoreText;
    public static int Coins { get; private set; }
    public string nameOfMaxScoreSave { get; } = "MaxScoreValueSave";
    public static Resource Instance;


    void Start() {
        Coins = 0;
        Instance = this;
        _coinsText.text = $"{Coins}";
        if (PlayerPrefs.HasKey(nameOfMaxScoreSave))
            UpdateMaxScore();
    }

    public void ChangeValue(int value) {
        Coins += value;
        _coinsText.text = $"{Coins}";
    }

    public void SetValue(int value) {
        Coins = value;
        _coinsText.text = $"{Coins}";
    }

    public bool TryBuy(int price) {
        if (Coins < price) {
            AudioManager.Instance.PlaySoundNEM();
            InformationText.Instance.Activate();
            return false;
        }
        ChangeValue(-price);
        GameManager.Instance.DivCoinScore(price);
        AudioManager.Instance.PlaySoundBuy();
        return true;
    }

    public void SaveMaxValue(int value) {
        if (value > PlayerPrefs.GetInt(nameOfMaxScoreSave)) {
            PlayerPrefs.SetInt(nameOfMaxScoreSave, value);
            PlayerPrefs.Save();
            UpdateMaxScore();
        }
    }

    public void UpdateMaxScore() {
        _maxScoreText.text = $"{Localization.Instance.GetRightPhase(2)}: {PlayerPrefs.GetInt(nameOfMaxScoreSave)}";
    }
}
