using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AircraftBullet : MonoBehaviour
{
    private float speed;
    private Vector2 direction;
    private new Rigidbody2D rigidbody2D;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        gameObject.SetActive(false);
    }
    public void Setup(Vector2 direction, float speed)
    {
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        transform.localPosition = Vector2.zero;
        gameObject.SetActive(true);
        this.direction = direction;
        this.speed = speed;
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Weapon>())
        {
            Destroy(other.gameObject);
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            gameObject.SetActive(false);
        }
        else if (other.gameObject.GetComponent<Wall>())
        {
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            gameObject.SetActive(false);
        }
    }
}
