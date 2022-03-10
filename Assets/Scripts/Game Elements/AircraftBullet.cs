using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AircraftBullet : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffect;
    [SerializeField] private SpriteRenderer _model;
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
        if (other.gameObject.GetComponent<Weapon>() || other.gameObject.GetComponent<Wall>())
        {
            if (other.gameObject.GetComponent<Weapon>())
                Destroy(other.gameObject);
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            Activate();
        }
    }

    void Activate()
    {
        _model.enabled = false;
        _explosionEffect.SetActive(true);
        Invoke(nameof(TurnOff), 1f);
    }

    void TurnOff()
    {
        _model.enabled = true;
        _explosionEffect.SetActive(false);
        gameObject.SetActive(false);
    }
}
