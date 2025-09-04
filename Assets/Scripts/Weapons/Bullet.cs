using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerController player;
    private Vector2 startPosition;
    private Vector2 direction;

    private float speed;
    private float lifeDistance;
    private int damage;
    private bool isDestroyable;
    private bool isActive = false;
    private bool isPlayerBullet = true;

    private void Start()
    {
        player = GameManager.Instance.GetPlayerController();
    }

    private void Update()
    {
        if (!isActive) return;

        transform.Translate(direction * speed * Time.deltaTime);

        if (Vector2.Distance(startPosition, transform.position) >= lifeDistance)
        {
            Deactivate();
        }
        else
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);

            if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
            {
                Deactivate();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;

        if (isPlayerBullet)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy_Shooter enemy = collision.GetComponent<Enemy_Shooter>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);

                    if (isDestroyable)
                    {
                        Deactivate();
                    }
                }
            }
            else
            {
                Deactivate();
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                if (player != null)
                {
                    player.TakeDamage(damage);

                    if (isDestroyable)
                    {
                        Deactivate();
                    }
                }
            }
            else
            {
                Deactivate();
            }
        }

    }

    public void Activate(Vector2 startPosition, Vector2 direction, float speed, float lifeDistance, int damage, bool isDestroyable, bool isPlayerWeapon)
    {
        transform.position = startPosition;
        this.startPosition = startPosition;
        this.direction = direction.normalized;
        this.speed = speed;
        this.lifeDistance = lifeDistance;
        this.damage = damage;
        this.isDestroyable = isDestroyable;
        this.isPlayerBullet = isPlayerWeapon;

        if (!isPlayerWeapon)
        {
            damage = 1;
        }

        isActive = true;
        gameObject.SetActive(true);
    }

    private void Deactivate()
    {
        isActive = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
