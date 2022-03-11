using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : Weapon
{
    [SerializeField] private float _speed = 0.1f;
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private GameObject _forceObject;
    [SerializeField] private SpriteRenderer _model;
    private bool launch;

    private void FixedUpdate()
    {
        if (launch)
            transform.Translate(Vector2.up * _speed);
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 20f);
            if (hit.rigidbody)
                if (hit.rigidbody.GetComponent<BodyPart>())
                {
                    ClearDictionary();
                    AudioManager.Instance.PlaySoundLaunch();
                    launch = true;
                    Destroy(gameObject, 5f);
                }
        }
    }

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
    }
}
