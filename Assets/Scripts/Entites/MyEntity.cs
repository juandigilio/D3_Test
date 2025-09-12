using UnityEngine;

public abstract class MyEntity : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected bool isGrounded;
    protected int availableLives;
    protected bool jumped = false;
    protected bool doubleJumped = false;

    private float rayLength;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from this gameobject");
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("Collider2D component missing from this gameobject");
        }

        rayLength = col.bounds.extents.y;
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        isGrounded = false;
        if (rb.linearVelocityY > 0) return;

        float extraHeight = 0.1f;

        Debug.DrawRay(transform.position, Vector2.down * (rayLength + extraHeight), Color.green);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, rayLength + extraHeight);

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.CompareTag("Player"))
            {
                isGrounded = true;
                jumped = false;
                doubleJumped = false;
                break;
            }
        }
    }

    protected void TakeDamage(int damage)
    {
        availableLives -= damage;

        if (availableLives <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
