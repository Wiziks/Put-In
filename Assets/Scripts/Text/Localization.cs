using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Language
{
    Ukrainian,
    Russian,
    English
}

[System.Serializable] public class PhraseArray { public string[] PhraseType = new string[3]; }

public class Localization : MonoBehaviour
{
    [SerializeField] private Sprite _ukr;
    [SerializeField] private Sprite _rus;
    [SerializeField] private Sprite _eng;
    [SerializeField] private Image _changeButton;
    [SerializeField] private TextMeshProUGUI _settingsLabel;
    [SerializeField] private TextMeshProUGUI _loseLabel;
    [SerializeField] private TextMeshProUGUI _continueLabel;
    [SerializeField] private TextMeshProUGUI _finishLabel;
    [SerializeField] private TextMeshProUGUI _question;
    [SerializeField] private TextMeshProUGUI _yes;
    [SerializeField] private TextMeshProUGUI _no;
    [SerializeField] private TextMeshProUGUI _succesfullText;
    [Header("Rate Panel")]
    [SerializeField] private TextMeshProUGUI _headerRatePanel;
    [SerializeField] private TextMeshProUGUI _textRatePanel;
    [SerializeField] private TextMeshProUGUI _buttonRatePanel;
    [Header("Start Game Text")]
    [SerializeField] private TextMeshProUGUI _startGameText;

    private Language language;
    private string nameOfLanguageTypeSave = "LanguageTypeSave";
    public static Localization Instance;

    [SerializeField] private PhraseArray[] PhraseArray = new PhraseArray[8];

    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey(nameOfLanguageTypeSave))
        {
            string language = PlayerPrefs.GetString(nameOfLanguageTypeSave);
            if (language == "Russian")
            {
                this.language = Language.Russian;
                _changeButton.sprite = _rus;
            }
            else if (language == "English")
            {
                this.language = Language.English;
                _changeButton.sprite = _eng;
            }
            else if (language == "Ukrainian")
            {
                this.language = Language.Ukrainian;
                _changeButton.sprite = _ukr;
            }
            UpdateAll();
        }
    }

    public void ChangeLanguage()
    {
        if (language == Language.Ukrainian)
        {
            language = Language.Russian;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "Russian");
            PlayerPrefs.Save();
            _changeButton.sprite = _rus;
        }
        else if (language == Language.Russian)
        {
            language = Language.English;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "English");
            PlayerPrefs.Save();
            _changeButton.sprite = _eng;
        }
        else if (language == Language.English)
        {
            language = Language.Ukrainian;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "Ukrainian");
            PlayerPrefs.Save();
            _changeButton.sprite = _ukr;
        }
        UpdateAll();
    }

    public Language GetLanguage() { return language; }

    public string GetRightPhase(int index)
    {
        return PhraseArray[index].PhraseType[(int)language];
    }

    void UpdateAll()
    {
        if (SnapScrolling.Instance)
            SnapScrolling.Instance.RefreshAll();
        if (Resource.Instance)
            Resource.Instance.UpdateMaxScore();
        UpdateSettings();
        UpdateLosePanel();
        UpdateRatePanel();
        UpdateExitMenu();
        _succesfullText.text = $"{GetRightPhase(32)}!";
        _startGameText.text = $"{GetRightPhase(36)}";
    }

    void UpdateSettings()
    {
        _settingsLabel.text = $"{GetRightPhase(9)}";
    }

    void UpdateLosePanel()
    {
        _loseLabel.text = $"{GetRightPhase(10)}";
        _continueLabel.text = $"{GetRightPhase(11)}";
        _finishLabel.text = $"{GetRightPhase(12)}";
    }

    void UpdateExitMenu()
    {
        _question.text = $"{GetRightPhase(22)}?";
        _yes.text = $"{GetRightPhase(23)}";
        _no.text = $"{GetRightPhase(24)}";
    }

    void UpdateRatePanel()
    {
        _headerRatePanel.text = $"{GetRightPhase(33)}!";
        _textRatePanel.text = $"{GetRightPhase(34)}";
        _buttonRatePanel.text = $"{GetRightPhase(35)}";
    }

    public void SetLanguage(Language value)
    {
        if (value == Language.Ukrainian)
        {
            language = Language.Ukrainian;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "Ukrainian");
            PlayerPrefs.Save();
        }
        else if (value == Language.Russian)
        {
            language = Language.Russian;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "Russian");
            PlayerPrefs.Save();
        }
        else if (value == Language.English)
        {
            language = Language.English;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "English");
            PlayerPrefs.Save();
        }
        UpdateAll();
    }
}
