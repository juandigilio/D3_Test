using System;
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
    private int currentWeapon = 1;

    [SerializeField] private Weapon weapon;
    [SerializeField] private Weapon pistol;
    [SerializeField] private Weapon automatic;
    [SerializeField] private Weapon rifle;

    private void OnEnable()
    {
        GameManager.Instance.RegisterPlayerController(this);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        weapon = pistol;
        weapon.gameObject.SetActive(true);
        automatic.gameObject.SetActive(false);
        rifle.gameObject.SetActive(false);
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
        Aim();
    }

    public void Shoot()
    {
        if (!weapon.HasAmmo())
        {
            NextWeapon();
        }

        Vector2 shootDirection = (sight.transform.localPosition - weapon.GetFirePoint().transform.position).normalized;

        weapon.Shoot(shootDirection);
    }

    public void NextWeapon()
    {
        currentWeapon++;
        if (currentWeapon > 3) currentWeapon = 1;

        SwitchWeapon(NextWeapon);
    }

    public void PreviousWeapon()
    {
        currentWeapon--;
        if (currentWeapon < 1) currentWeapon = 3;

        SwitchWeapon(PreviousWeapon);
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
            }
            else if (direction < 0)
            {
                //transform.localScale = new Vector3(-1, 1, 1);
                //sight.transform.localScale = new Vector3(-1, 1, 1);
            }

            //Vector3 scale = transform.localScale;
            //scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
            //transform.localScale = scale;
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
            sight.transform.localPosition = new Vector2(weapon.GetFirePoint().transform.position.x, weapon.GetFirePoint().transform.position.y) + (inputDirection * sightOffset);
        }
        //else
        //{
        //    sight.transform.localPosition = new Vector2(direction * sightOffset, 0);
        //}

    }

    private void SwitchWeapon(Action onNoAmmo)
    {
        switch (currentWeapon)
        {
            case 1:
            {
                if (pistol.HasAmmo())
                {
                    pistol.gameObject.SetActive(true);
                    automatic.gameObject.SetActive(false);
                    rifle.gameObject.SetActive(false);
                    weapon = pistol;
                }
                else
                {
                    onNoAmmo?.Invoke();
                }
                break;
            }
            case 2:
            {
                if (automatic.HasAmmo())
                {
                    pistol.gameObject.SetActive(false);
                    automatic.gameObject.SetActive(true);
                    rifle.gameObject.SetActive(false);
                    weapon = automatic;
                }
                else
                {
                    onNoAmmo?.Invoke();
                }
                break;
            }
            case 3:
            {
                if (rifle.HasAmmo())
                {
                    pistol.gameObject.SetActive(false);
                    automatic.gameObject.SetActive(false);
                    rifle.gameObject.SetActive(true);
                    weapon = rifle;
                }
                else
                {
                    onNoAmmo?.Invoke();
                }
                break;
            }
        }
    }
}
