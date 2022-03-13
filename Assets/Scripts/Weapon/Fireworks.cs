using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : Weapon
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private GameObject _forceObject;
    [SerializeField] private SpriteRenderer _model;
    [SerializeField] private Collider2D ignoreCollider;
    [SerializeField] private ParticleSystem _launchParticles;

    public override void DivStrength()
    {
        Activate();
    }

    public void Activate()
    {
        launch = false;
        _model.enabled = false;
        _explosionEffect.SetActive(true);
        _forceObject.SetActive(true);
        _launchParticles.Stop();
        AudioManager.Instance.PlaySoundExplosion();
        Destroy(gameObject, 1f);
    }
    bool launch;
    float speed, lerpRate;
    void Update()
    {
        if (!launch) return;

        transform.Translate(transform.up * speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * lerpRate * Time.deltaTime);

        if (transform.position.y <= -6f || transform.position.x <= -7f || transform.position.x >= 7f || transform.position.y >= 6f)
            Activate();
    }

    public void Setup(float speed, float lerpRate)
    {
        this.speed = speed;
        this.lerpRate = lerpRate;
        launch = true;
        AudioManager.Instance.PlaySoundLaunch();
    }
}
