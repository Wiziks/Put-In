using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bayraktar : Weapon
{
    [SerializeField] private float _timeOfStan = 3f;
    [SerializeField] private float _speed = 10f;
    private GameObject _explosionEffect;
    Vector2 direction;
    bool bomb;

    void Start()
    {
        _explosionEffect = Aircraft.Instance.transform.GetChild(0).gameObject;
        transform.position = new Vector3(-10, 10, 0);
        direction = (Aircraft.Instance.transform.position - transform.position).normalized;
        Aircraft.Instance.SetSpeedMultiplier(0);
    }

    void Update()
    {
        transform.Translate(direction * _speed * Time.deltaTime);
        if (transform.position.y < Aircraft.Instance.transform.position.y)
        {
            if (!bomb)
            {
                bomb = true;
                CameraManager.Instance.Zoom(5);
                _explosionEffect.SetActive(true);
                StartCoroutine(Bombing());
            }
        }
        else
            CameraManager.Instance.ConstantTarget(Vector2.zero);
    }

    IEnumerator Bombing()
    {
        yield return new WaitForSeconds(_timeOfStan);
        _explosionEffect.SetActive(false);
        Aircraft.Instance.SetSpeedMultiplier(1);
        Destroy(gameObject);
    }
}
