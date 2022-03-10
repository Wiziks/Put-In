using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Weapon
{
    [SerializeField] private Fireworks _fireworkPrefab1;
    [SerializeField] private Fireworks _fireworkPrefab2;
    [SerializeField] private Fireworks _fireworkPrefab3;

    public void StartLaunch()
    {
        StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        for (int i = 0; i < _strenght; i++)
        {
            Instantiate(_fireworkPrefab1, transform.position, Quaternion.identity, transform).Setup(Random.Range(3f, 5f), Random.Range(-50f, -10f));
            Instantiate(_fireworkPrefab2, transform.position, Quaternion.identity, transform).Setup(Random.Range(3f, 5f), 0);
            Instantiate(_fireworkPrefab3, transform.position, Quaternion.identity, transform).Setup(Random.Range(3f, 5f), Random.Range(10f, 50f));
            yield return new WaitForSeconds(3);
        }
        Destroy(gameObject);
    }
    public override void DivStrength() { }
}
