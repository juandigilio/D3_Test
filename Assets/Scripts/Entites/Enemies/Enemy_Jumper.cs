using UnityEngine;

public class Enemy_Jumper : Enemy
{
    [SerializeField] private float jumpingRange = 7f;
    [SerializeField] private float minJumpHeight = 2f;
    [SerializeField] private float attackRange = 0.1f;
    [SerializeField] private float attackCooldown = 1f;

    private void Start()
    {
        availableLives = 1;
    }

    private void FixedUpdate()
    {
        UpdateCurrentState();
    }

    private void UpdateCurrentState()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (isGrounded)
        {
            if (distance <= walkingRange)
            {
                WalkTowardsPlayer();
            }
            else if (distance <= jumpingRange)
            {
                JumpTowardsPlayer();
            }
            else
            {
                Patrol();
            }
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

    private void WalkTowardsPlayer()
    {
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.linearVelocity = new Vector2(dir * walkSpeed, rb.linearVelocity.y);
    }

    private void JumpTowardsPlayer()
    {

    }
}