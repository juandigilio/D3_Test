using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject sight;
    [SerializeField] private float sightOffset = 1f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private float direction;
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
        if (inputDirection != Vector2.zero)
        {
            Move();
        }
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        inputDirection = newDirection.normalized;

        if (inputDirection != Vector2.zero)
        {
            direction = inputDirection.x;
            Move();
        }
        else
        {
            //stop inertia
        }
    }

    private void Move()
    {
        if (inputDirection != Vector2.zero)
        {
            Vector2 movement = new Vector2(inputDirection.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = movement;

            Vector2 aimingDirection = 
            sight.transform.localPosition = inputDirection;
        }
    }
}
