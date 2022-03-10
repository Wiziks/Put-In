using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : Weapon
{
    void Update()
    {
        if (transform.position.x > 0)
        {
            _sprite.flipY = true;
            Collider.offset = new Vector2(0.25f, 0);
            _sprite.transform.localPosition = new Vector2(0.5f, 0);
        }
        else
        {
            _sprite.flipY = false;
            Collider.offset = new Vector2(-0.25f, 0);
            _sprite.transform.localPosition = new Vector2(-0.5f, 0);
        }
    }
}
