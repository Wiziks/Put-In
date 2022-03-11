using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Weapon
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private GameObject _forceObject;
    [SerializeField] private SpriteRenderer _model;

    public override void DivStrength()
    {
        Activate();
    }

    public void Activate()
    {
        _model.enabled = false;
        _explosionEffect.SetActive(true);
        _forceObject.SetActive(true);
        AudioManager.Instance.PlaySoundExplosion();
        Destroy(gameObject, 1f);
    }
}
