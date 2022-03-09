using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    private Weapon weaponToBuy;

    public void SetWeapon(Weapon weapon)
    {
        this.weaponToBuy = weapon;
    }

    public void TryBuy()
    {
        if (!Resource.Instance.TryBuy(weaponToBuy.GetPrice())) return;

        SnapScrolling.Instance.Refresh();
        weaponToBuy.Buy();
        gameObject.SetActive(false);
    }
}
