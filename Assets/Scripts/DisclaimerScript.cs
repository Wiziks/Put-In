using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisclaimerScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Localization _localization;
    [SerializeField] private TutorialScript _tutorialScript;

    [Header("Panels")]
    [SerializeField] private GameObject _languagePanel;
    [SerializeField] private GameObject _disclaimerPanel;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _disclaimerText;
    [SerializeField] private string _textUKR;
    [SerializeField] private string _textRUS;
    [SerializeField] private string _textENG;

    void Start()
    {
        if (PlayerPrefs.HasKey(_tutorialScript.GetKeyName()))
            if (PlayerPrefs.GetInt(_tutorialScript.GetKeyName()) == 0)
                _languagePanel.SetActive(true);
            else
                ShowDisclaimerText();
        else
            _languagePanel.SetActive(true);
    }

    void ShowDisclaimerText()
    {
        _languagePanel.SetActive(false);
        _disclaimerPanel.SetActive(true);
        _disclaimerText.text = "<b>Disclaimer</b>\n\n";
        if (_localization.GetLanguage() == Language.Ukrainian)
            _disclaimerText.text += _textUKR;
        else if (_localization.GetLanguage() == Language.Russian)
            _disclaimerText.text += _textRUS;
        else if (_localization.GetLanguage() == Language.English)
            _disclaimerText.text += _textENG;
    }

    public void SetUkrainianLanguage()
    {
        _localization.SetLanguage(Language.Ukrainian);
        ShowDisclaimerText();
    }

    public void SetRussianLanguage()
    {
        _localization.SetLanguage(Language.Russian);
        ShowDisclaimerText();
    }

    public void SetEnglishLanguage()
    {
        _localization.SetLanguage(Language.English);
        ShowDisclaimerText();
    }
}
