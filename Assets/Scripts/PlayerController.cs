using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private int currentWeapon = 0;
    private bool isShooting = false;
    private int availableLives = 8;

    private bool jumped = false;
    private bool doubleJumped = false;
    private float rayLength;

    //1-Pistol 2-Automatic 3-Rifle
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayerController(this);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        weapons[0].gameObject.SetActive(true);
        weapons[1].gameObject.SetActive(false);
        weapons[2].gameObject.SetActive(false);

        Collider2D col = GetComponent<Collider2D>();
        rayLength = col.bounds.extents.y;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        CheckGrounded();

        Move();
        Aim();
        Shoot();
    }

    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
    }

    public void Jump()
    {
        if (!doubleJumped)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            if (!jumped)
            {
                jumped = true;
            }
            else
            {
                doubleJumped = true;
            }
        }
    }

    public void NextWeapon()
    {
        currentWeapon++;
        if (currentWeapon > 2) currentWeapon = 0;
        SwitchWeapon(NextWeapon);
    }

    public void PreviousWeapon()
    {
        currentWeapon--;
        if (currentWeapon < 0) currentWeapon = 2;
        SwitchWeapon(PreviousWeapon);
    }

    public void TakeDamage(int damage)
    {
        availableLives -= damage;

        if (availableLives <= 0)
        {
            gameObject.SetActive(false);
            //GameManager.Instance.GameOver();
        }
    }

    public void SetInputDirection(Vector2 newDirection)
    {
        inputDirection = newDirection.normalized;

        if (inputDirection.x != 0)
        {
            direction = inputDirection.x;

            if (direction > 0)
            {
                //transform.localScale = new Vector3(1, 1, 1);
                //sight.transform.localScale = new Vector3(1, 1, 1);
                //weapons[currentWeapon].transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction < 0)
            {
                //transform.localScale = new Vector3(-1, 1, 1);
                //sight.transform.localScale = new Vector3(-1, 1, 1);
                //weapons[currentWeapon].transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void CheckGrounded()
    {
        if (rb.linearVelocityY > 0) return;

        float extraHeight = 0.1f;

        Debug.DrawRay(transform.position, Vector2.down * (rayLength + extraHeight), Color.green);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, rayLength + extraHeight);

        foreach (RaycastHit2D hit in hits)
        {
            if (!hit.collider.CompareTag("Player"))
            {
                jumped = false;
                doubleJumped = false;
                break;
            }
        }
    }

    private void Shoot()
    {
        if (isShooting)
        {
            if (!weapons[currentWeapon].HasAmmo())
            {
                NextWeapon();
            }

            Vector2 shootDirection = (sight.transform.localPosition - weapons[currentWeapon].GetFirePointLocalPos()).normalized;

            weapons[currentWeapon].Shoot(shootDirection);
        }
    }

    private void Move()
    {
        if (inputDirection != Vector2.zero)
        {
            Vector2 movement = new Vector2(inputDirection.x * moveSpeed, rb.linearVelocity.y);
            rb.linearVelocity = movement;
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void Aim()
    {
        float angle;
        Vector2 newDisplacement = weapons[currentWeapon].GetFirePointLocalPos();

        if (inputDirection != Vector2.zero)
        {
            float rawAngle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;

            angle = Mathf.Round(rawAngle / 45f) * 45f;

            float rad = angle * Mathf.Deg2Rad;
            Vector2 quantizedDirection = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

            weapons[currentWeapon].AimAt(angle);
            sight.transform.localPosition = newDisplacement + (quantizedDirection * sightOffset);
        }
        else
        {
            if (direction >= 0) angle = 0;
            else angle = 180;

            weapons[currentWeapon].AimAt(angle);
            sight.transform.localPosition = new Vector2(direction * sightOffset, 0) + newDisplacement;
        }
    }

    private void SwitchWeapon(Action onNoAmmo)
    {
        if (weapons[currentWeapon].HasAmmo())
        {
            weapons[currentWeapon].gameObject.SetActive(true);

            for (int i = 0; i < weapons.Count; i++)
            {
                if (i != currentWeapon)
                {
                    weapons[i].gameObject.SetActive(false);
                }
            }
            return;
        }
        else
        {
            onNoAmmo?.Invoke();
        }
    }
}