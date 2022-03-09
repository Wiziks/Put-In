using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(transform.GetChild(i).GetComponent<BuildManager>().GetPrefab().GetActive());
    }
}
