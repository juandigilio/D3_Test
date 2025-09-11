using UnityEngine;

public class Enemy_Shooter : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float speed = 20f;
    [SerializeField] private int health = 1;

    private PlayerController playerController;
    private Rigidbody2D rigidBody;
    [SerializeField] private bool movingRight = true;
    [SerializeField] private bool isShooting = false;
    private float shootDistance;



    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody2D component not found on the enemy.");
        }

        playerController = GameManager.Instance.GetPlayerController();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found in GameManager.");
        }

        if (leftPoint == null || rightPoint == null)
        {
            Debug.LogError("Patrol points not assigned.");
        }

        transform.position = leftPoint.position;

        shootDistance = weapon.GetWeaponRange();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        Patroll();
        Attack();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void Patroll()
    {
        if (isShooting) return;

        Vector2 direction;

        if (movingRight)
        {
            direction = Vector2.right;

            if (transform.position.x >= rightPoint.position.x)
            {
                movingRight = false;
            }
        }
        else
        {
            direction = Vector2.left;

            if (transform.position.x <= leftPoint.position.x)
            {
                movingRight = true;
            }
        }

        Vector2 movemenDirection = new Vector2(direction.x * speed, rigidBody.linearVelocity.y);
        rigidBody.linearVelocity = movemenDirection;
    }

    private void Attack()
    {
        if ((movingRight && playerController.transform.position.x > transform.position.x) || 
            (!movingRight && playerController.transform.position.x < transform.position.x))
        {
            Vector2 direction = (playerController.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, playerController.transform.position);

            if (distance > shootDistance) 
            {
                isShooting = false;
                return;
            }

            Debug.DrawRay(weapon.GetFirePointWorldPos(), direction * distance, Color.red, 0.1f);
            RaycastHit2D[] hits = Physics2D.RaycastAll(weapon.GetFirePointWorldPos(), direction, distance);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null)
                {
                    continue;
                }

                if (hit.collider.CompareTag("Bullet"))
                {
                    continue;
                }

                if (hit.collider.CompareTag("Player"))
                {
                    Shoot(direction);
                    isShooting = true;
                }
                else
                {
                    isShooting = false;
                }

                break;
            }
        }
        else
        {
            Debug.Log("No estoy mirando al player");
            isShooting = false;
        }
    }

    private void Shoot(Vector2 direction)
    {
        weapon.Shoot(direction);
    }
}
