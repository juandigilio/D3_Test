using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector2 startPosition;
    Vector2 direction;

    float speed;
    float lifeDistance;
    int damage;
    bool isDestroyable;
    bool isActive = false;

    private void Start()
    {

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

        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

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

    public void Activate(Vector2 startPosition, Vector2 direction, float speed, float lifeDistance, int damage, bool isDestroyable)
    {
        transform.position = startPosition;
        this.startPosition = startPosition;
        this.direction = direction.normalized;
        this.speed = speed;
        this.lifeDistance = lifeDistance;
        this.damage = damage;
        this.isDestroyable = isDestroyable;

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
