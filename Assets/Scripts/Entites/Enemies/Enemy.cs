using UnityEngine;

public abstract class Enemy : MyEntity
{
    [SerializeField] protected Transform leftPoint;
    [SerializeField] protected Transform rightPoint;
    [SerializeField] protected float walkSpeed = 5f;
    [SerializeField] protected int health = 1;
    [SerializeField] protected int damage = 1;

    protected PlayerController player;
    protected bool movingRight = true;
    protected bool isAttacking = false;


    private void Start()
    {
        player = GameManager.Instance.GetPlayerController();
        transform.position = leftPoint.position;
    }


    protected void Patrol()
    {
        Vector2 dir;
        if (movingRight)
        {
            dir = Vector2.right;
            if (transform.position.x >= rightPoint.position.x) movingRight = false;
        }
        else
        {
            dir = Vector2.left;
            if (transform.position.x <= leftPoint.position.x) movingRight = true;
        }

        rb.linearVelocity = new Vector2(dir.x * walkSpeed, rb.linearVelocity.y);
    }

    protected void Attack()
    {
        player.TakeDamage(damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) gameObject.SetActive(false);
    }
}

