using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireworks : Weapon
{
    bool launch;
    float speed, lerpRate;
    void Update()
    {
        if (!launch) return;

        transform.Translate(transform.up * speed * Time.deltaTime);
        transform.Rotate(Vector3.forward * lerpRate * Time.deltaTime);

        if (transform.position.y <= -6f || transform.position.y <= -7f || transform.position.y >= 7f || transform.position.y >= 6f)
            Destroy(gameObject);
    }

    public void Setup(float speed, float lerpRate)
    {
        this.speed = speed;
        this.lerpRate = lerpRate;
        launch = true;
    }
}
