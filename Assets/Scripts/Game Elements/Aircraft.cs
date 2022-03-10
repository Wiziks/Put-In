using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Right,
    Left
}

public class Aircraft : MonoBehaviour
{
    [Header("Aircraft")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Transform target;
    [SerializeField] private float _timeToMad;

    [Header("Bullet")]
    [SerializeField] private AircraftBullet _bullet;
    [SerializeField] private float _startSpeed;
    private float speedMultiplier = 1f;
    private Direction current;
    private float timer;
    public static Aircraft Instance;

    void Start()
    {
        Instance = this;
        this.enabled = false;
    }

    void Update()
    {
        if (transform.position.x > target.position.x)
        {
            _sprite.flipX = false;
            current = Direction.Left;
        }
        else if (transform.position.x < target.position.x)
        {
            _sprite.flipX = true;
            current = Direction.Right;
        }

        if (current == Direction.Right)
        {
            RaycastHit2D hitRight1 = Physics2D.Raycast(transform.position + Vector3.up * 0.45f, Vector2.right, 1f);
            RaycastHit2D hitRight2 = Physics2D.Raycast(transform.position - Vector3.up * 0.45f, Vector2.right, 1f);
            Debug.DrawRay(transform.position + Vector3.up * 0.45f, Vector2.right, Color.green);
            Debug.DrawRay(transform.position - Vector3.up * 0.45f, Vector2.right, Color.green);
            Move(hitRight1, hitRight2);
        }
        else if (current == Direction.Left)
        {
            RaycastHit2D hitLeft1 = Physics2D.Raycast(transform.position + Vector3.up * 0.45f, Vector2.left, 1f);
            RaycastHit2D hitLeft2 = Physics2D.Raycast(transform.position - Vector3.up * 0.45f, Vector2.left, 1f);
            Debug.DrawRay(transform.position + Vector3.up * 0.45f, Vector2.left, Color.green);
            Debug.DrawRay(transform.position - Vector3.up * 0.45f, Vector2.left, Color.green);
            Move(hitLeft1, hitLeft2);
        }
    }

    void Adjustment()
    {
        Vector2 adjustmentVector = Vector2.zero;
        RaycastHit2D hitUp1 = Physics2D.Raycast(transform.position + Vector3.right, Vector2.up, 1f);
        RaycastHit2D hitUp2 = Physics2D.Raycast(transform.position - Vector3.right, Vector2.up, 1f);
        RaycastHit2D hitDown1 = Physics2D.Raycast(transform.position + Vector3.right, -Vector2.up, 1f);
        RaycastHit2D hitDown2 = Physics2D.Raycast(transform.position - Vector3.right, -Vector2.up, 1f);
        if (hitUp1)
            adjustmentVector += Vector2.left + Vector2.down;
        if (hitUp2)
            adjustmentVector += Vector2.right + Vector2.down;
        if (hitDown1)
            adjustmentVector += Vector2.left + Vector2.up;
        if (hitDown2)
            adjustmentVector += Vector2.right + Vector2.up;

        transform.Translate(adjustmentVector * speedMultiplier * GameManager.Instance.Speed * Time.deltaTime);
    }

    void Move(RaycastHit2D hit1, RaycastHit2D hit2)
    {
        if (_bullet.gameObject.activeSelf) return;

        if (hit1 || hit2)
        {
            if (hit1)
            {
                if (hit1.collider.GetComponent<CircleSelector>()) return;
            }
            else if (hit2)
            {
                if (hit2.collider.GetComponent<CircleSelector>()) return;
            }
            AvoidObstacle();

            timer += Time.deltaTime;
            if (timer > _timeToMad)
                Shot();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speedMultiplier * GameManager.Instance.Speed * Time.deltaTime);
            timer = 0;
        }
    }

    void AvoidObstacle()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, 1.5f);
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, 1.5f);
        Debug.DrawRay(transform.position, Vector2.up * 0f, Color.red);
        Debug.DrawRay(transform.position, Vector2.down * 0f, Color.red);
        if (target.position.y < transform.position.y)
        {
            if (!hitDown)
            {
                transform.Translate(Vector2.down * speedMultiplier * GameManager.Instance.Speed * Time.deltaTime);
            }
            else
                Shot();
        }
        else
        {
            if (!hitUp)
            {
                transform.Translate(Vector2.up * speedMultiplier * GameManager.Instance.Speed * Time.deltaTime);
            }
            else
                Shot();
        }
    }

    void Shot()
    {
        Vector2 direction = Vector2.right;
        if (current == Direction.Left)
            direction = Vector2.left;
        _bullet.Setup(direction, _startSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<BodyPart>())
            GameManager.Instance.GameOver();
    }

    public void SetSpeedMultiplier(float value)
    {
        speedMultiplier = value;
    }
}
