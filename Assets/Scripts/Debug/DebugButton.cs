using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Type
{
    Cost,
    Strenght,
    Price,
    DamageKoeficient,
    Active,
    StartSpeed,
    DeltaSpeed,
    ScoreInCoins
}

public class DebugButton : MonoBehaviour
{
    [SerializeField] private Type type;
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameManager gameManager;
    void Start()
    {
        if (type == Type.Active)
        {
            Toggle toggle = GetComponent<Toggle>();
            toggle.enabled = weapon.GetActive();
        }
        else if(type == Type.StartSpeed)
        {
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            inputField.text = gameManager.GetStartSpeed().ToString();
        }
        else if(type == Type.DeltaSpeed)
        {
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            inputField.text = gameManager.GetDeltaSpeed().ToString();
        }
        else if(type == Type.ScoreInCoins)
        {
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            inputField.text = gameManager.GetScoreMultiplier().ToString();
        }
        else
        {
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            if (type == Type.Cost)
            {
                inputField.text = weapon.GetCost().ToString();
            }
            else if (type == Type.Strenght)
            {
                inputField.text = weapon.GetStrenght().ToString();
            }
            else if (type == Type.Price)
            {
                inputField.text = weapon.GetPrice().ToString();
            }
            else if (type == Type.DamageKoeficient)
            {
                inputField.text = weapon.GetDamageKoeficient().ToString();
            }
        }


    }
}
