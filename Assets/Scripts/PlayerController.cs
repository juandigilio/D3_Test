using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject sight;
    [SerializeField] private float sightOffset = 1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private float direction = 1f;
    private Vector2 inputDirection;

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayerController(this);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
        Aim();
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        inputDirection = newDirection.normalized;

        if (inputDirection.x != 0)
        {
            direction = inputDirection.x;
        }
    }

    private void Move()
    {
        if (inputDirection != Vector2.zero)
        {
            Vector2 movement = new Vector2(inputDirection.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = movement;

            sight.transform.localPosition = inputDirection * sightOffset;
        }
        else
        {
            //stop inertia
        }
    }

    private void Aim()
    {
        if (inputDirection != Vector2.zero)
        {
            sight.transform.localPosition = inputDirection * sightOffset;
        }
        else
        {
            sight.transform.localPosition = new Vector2(direction * sightOffset, 0);
        }
    }
}
