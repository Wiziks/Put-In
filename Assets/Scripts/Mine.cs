using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private GameObject _forceObject;
    [SerializeField] private SpriteRenderer _model;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<BodyPart>())
            Activate();
    }

    public void Activate()
    {
        _model.enabled = false;
        _explosionEffect.SetActive(true);
        _forceObject.SetActive(true);
        Destroy(gameObject, 1f);
    }
}
