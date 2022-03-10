using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DevelopmentScript : MonoBehaviour
{
    [SerializeField] private GameObject _passwordPanel;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private GameObject _settingsPanel;
    private TouchScreenKeyboard keyboard;

    public void CheckPasword(string value)
    {
        if (value == "243057")
        {
            _passwordPanel.SetActive(false);
            _settingsPanel.SetActive(true);
        }
    }

}
