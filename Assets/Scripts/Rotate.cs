using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float _speed;
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * _speed);
    }
}
