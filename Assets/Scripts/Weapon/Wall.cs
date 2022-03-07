using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float _borderVelocity = 10f;
    [SerializeField] private float _damageKoeficient = 1f;

    public float GetBorderVelocity() { return _borderVelocity; }
    public float GetKoeficient() { return _damageKoeficient; }
}
