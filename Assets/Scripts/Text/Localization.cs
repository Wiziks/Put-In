using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum LanguageType
{
    Ukrainian,
    Russian,
    English
}

public class Localization : MonoBehaviour
{
    [SerializeField] private Sprite _ukr;
    [SerializeField] private Sprite _rus;
    [SerializeField] private Sprite _eng;
    [SerializeField] private Image _changeButton;
    private LanguageType languageType;
    private string nameOfLanguageTypeSave = "LanguageTypeSave";
    void Start()
    {
        if (PlayerPrefs.HasKey(nameOfLanguageTypeSave))
        {
            string language = PlayerPrefs.GetString(nameOfLanguageTypeSave);
            if (language == "Russian")
            {
                languageType = LanguageType.Russian;
                _changeButton.sprite = _rus;
            }
            else if (language == "English")
            {
                languageType = LanguageType.English;
                _changeButton.sprite = _eng;
            }
            else if (language == "Ukrainian")
            {
                languageType = LanguageType.Ukrainian;
                _changeButton.sprite = _ukr;
            }
        }
    }

    public void ChangeLanguage()
    {
        if (languageType == LanguageType.Ukrainian)
        {
            languageType = LanguageType.Russian;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "Russian");
            PlayerPrefs.Save();
            _changeButton.sprite = _rus;
        }
        else if (languageType == LanguageType.Russian)
        {
            languageType = LanguageType.English;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "English");
            PlayerPrefs.Save();
            _changeButton.sprite = _eng;
        }
        else if (languageType == LanguageType.English)
        {
            languageType = LanguageType.Ukrainian;
            PlayerPrefs.SetString(nameOfLanguageTypeSave, "Ukrainian");
            PlayerPrefs.Save();
            _changeButton.sprite = _ukr;
        }

    }
}
