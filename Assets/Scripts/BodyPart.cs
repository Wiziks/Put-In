using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BodyPart : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 1f;
    Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void BecomeDynamic()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Weapon takeDamage = other.gameObject.GetComponent<Weapon>();
        if (takeDamage)
        {
            if (other.relativeVelocity.magnitude >= takeDamage.GetBorderVelocity())
            {
                GameManager.Instance.AddScore(takeDamage.GetKoeficient(), _damageMultiplier, transform.position);
            }
        }
    }
}
