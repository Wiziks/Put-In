using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInitializer : MonoBehaviour
{
    [SerializeField] private Weapon[] _weapons;

    private void Awake()
    {
        foreach (Weapon weapon in _weapons)
        {
            weapon.ActivateWeapon();
        }
    }
}
