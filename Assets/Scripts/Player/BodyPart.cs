using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BodyPart : MonoBehaviour
{
    [SerializeField] private float _damageMultiplier = 1f;
    public Rigidbody2D _rigidbody2D;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public void BecomeDynamic()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.gravityScale = 1f;
        _rigidbody2D.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    public void BecomeStatic()
    {
        if (_rigidbody2D.bodyType != RigidbodyType2D.Static)
            _rigidbody2D.velocity = Vector2.zero;
        _rigidbody2D.gravityScale = 0f;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Weapon takeDamage = other.gameObject.GetComponent<Weapon>();
        if (takeDamage)
        {
            GameManager.Instance.AddScore(takeDamage.GetKoeficient(), _damageMultiplier, transform.position);
            takeDamage.DivStrength();
            AudioManager.Instance.PlaySoundDamage();
            return;
        }

        Wall wall = other.gameObject.GetComponent<Wall>();
        if (wall)
        {
            if (other.relativeVelocity.magnitude >= wall.GetBorderVelocity())
            {
                AudioManager.Instance.PlaySoundDamage();
                GameManager.Instance.AddScore(wall.GetKoeficient(), _damageMultiplier, transform.position);
            }
        }
    }
}
