using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bayraktar : Weapon
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private GameObject _forceObject;

    void Start()
    {
        _explosionEffect.transform.position = CircleSelector.Instance.GetBodyPart().transform.position;
        _forceObject.transform.position = CircleSelector.Instance.GetBodyPart().transform.position;
        _explosionEffect.SetActive(true);
        _forceObject.SetActive(true);
        Destroy(gameObject, 1f);
    }

    
}
