using UnityEngine;

public class Enemy_Jumper : MonoBehaviour
{
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpCooldown = 1f;
    [SerializeField] private float maxJumpTime = 1f;
    [SerializeField] private int health = 1;
    [SerializeField] private float jumpingRange = 7f;
    [SerializeField] private float walkingRange = 2f;
    [SerializeField] private float attackRange = 0.1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float attackCooldown = 1f;

    private Rigidbody2D rb;
    private PlayerController player;
    private bool movingRight = true;
    private bool canJump = true;
    private bool isAttacking = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.GetPlayerController();
        transform.position = leftPoint.position;
    }

    private void FixedUpdate()
    {
        UpdateCurrentState();
    }

    private void UpdateCurrentState()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
  
        if (distance <= walkingRange)
        {
            WalkTowardsPlayer();
        }
        else if (distance <= jumpingRange && canJump)
        {
            JumpTowardsPlayer();
        }
        else
        {
            Patrol();
        }

        CheckAttackState(distance);
    }

    private void CheckAttackState(float distance)
    {
        if (distance <= attackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                InvokeRepeating(nameof(Attack), 0f, attackCooldown);
            }
        }
        else
        {
            if (isAttacking)
            {
                isAttacking = false;
                CancelInvoke(nameof(Attack));
            }
        }
    }

    private void Attack()
    {
        player.TakeDamage(damage);
    }

    private void WalkTowardsPlayer()
    {
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * walkSpeed, rb.linearVelocity.y);
    }

    private void JumpTowardsPlayer()
    {
        Vector2 start = transform.position;
        Vector2 target = player.transform.position;

        float dx = target.x - start.x;
        float dy = target.y - start.y;

        float t = maxJumpTime;
        float vx = dx / t;
        float vy = (dy + 0.5f * Physics2D.gravity.magnitude * t * t) / t;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(vx, vy), ForceMode2D.Impulse);

        canJump = false;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void Patrol()
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) gameObject.SetActive(false);
    }
}
