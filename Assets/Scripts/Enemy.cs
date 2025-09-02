using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int health = 1;
    [SerializeField] private int damage = 1;

    private PlayerController playerController;
    private Rigidbody2D rigidBody;
    private bool movingRight = true;
    private bool isShooting = false;



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
            direction = (transform.position - rightPoint.position).normalized;
            transform.LookAt(rightPoint.position);

            if (Vector2.Distance(transform.position, rightPoint.position) < 0.1f)
            {
                movingRight = false;
            }
        }
        else
        {
            direction = (transform.position - leftPoint.position).normalized;

            if (Vector2.Distance(transform.position, leftPoint.position) < 0.1f)
            {
                movingRight = true;
            }
        }
           
        rigidBody.AddForce(direction * speed * Time.deltaTime);
    }

    private void Attack()
    {
        if ((movingRight && playerController.transform.position.x > transform.position.x) || 
            (!movingRight && playerController.transform.position.x < transform.position.x))
        {
            Vector2 direction = (playerController.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, playerController.transform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("¡Tengo tiro directo al player!");

                    Shoot(direction);
                    isShooting = true;
                }
                else
                {
                    Debug.Log("Bloqueado por: " + hit.collider.name);

                    isShooting = false;
                }
            }
        }
    }

    private void Shoot(Vector2 direction)
    {
        weapon.Shoot(direction);
    }
}
