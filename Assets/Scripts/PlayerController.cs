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
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        Move();
        Aim();
        Shoot();
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

    public void SetShooting(bool shooting)
    {
        isShooting = shooting;
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
            angle = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            weapons[currentWeapon].AimAt(angle);
            sight.transform.localPosition = newDisplacement + (inputDirection * sightOffset);

            Debug.DrawRay(weapons[currentWeapon].GetFirePointWorldPos(), direction * inputDirection, Color.red, 0.1f);
        }
        else
        {
            if (direction >= 0) angle = 0;
            else angle = 180;

            weapons[currentWeapon].AimAt(angle);
            sight.transform.localPosition = new Vector2(direction * sightOffset, 0) + newDisplacement;
            Debug.DrawRay(weapons[currentWeapon].GetFirePointWorldPos(), 50 * inputDirection, Color.red, 0.1f);
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