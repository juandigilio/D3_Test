using System;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Automatic,
    Rifle,
}

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] int magazineSize;

    [SerializeField] float fireRate;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifeDistance;
    [SerializeField] int bulletDamage;
    [SerializeField] bool bulletIsDestroyable;

    [SerializeField] bool isPlayerWeapon;


    private float fireCooldown;
    private int currentAmmo;


    private void Awake()
    {
        SetWeaponType();
    }

    private void Update()
    {
        fireCooldown += Time.deltaTime;
    }

    private void FixedUpdate()
    {

    }

    public void AimAt(float angle)
    {
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void Shoot(Vector2 direction)
    {
        if (fireCooldown > fireRate)
        {
            fireCooldown = 0f;

            Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            newBullet.Activate(firePoint.position, direction, bulletSpeed, bulletLifeDistance, bulletDamage, bulletIsDestroyable, isPlayerWeapon);

            currentAmmo--;
        }
    }

    public void SetWeaponType()
    {
        if (weaponType == WeaponType.Pistol)
        {
            magazineSize = 999999;
            fireRate = 0.5f;
            bulletSpeed = 10f;
            bulletLifeDistance = 8f;
            bulletDamage = 1;
            bulletIsDestroyable = true;
        }
        else if (weaponType == WeaponType.Automatic)
        {
            magazineSize = 500;
            fireRate = 0.1f;
            bulletSpeed = 15f;
            bulletLifeDistance = 10f;
            bulletDamage = 1;
            bulletIsDestroyable = true;
        }
        else if (weaponType == WeaponType.Rifle)
        {
            magazineSize = 50;
            fireRate = 1f;
            bulletSpeed = 20f;
            bulletLifeDistance = 20f;
            bulletDamage = 3;
            bulletIsDestroyable = false;
        }

        currentAmmo = magazineSize;
    }

    public bool HasAmmo()
    {
        return currentAmmo > 0;
    }

    public Vector3 GetFirePointLocalPos()
    {
        return firePoint.localPosition;
    }

    public Vector3 GetFirePointWorldPos()
    {
        return firePoint.position;
    }

    internal float GetWeaponRange()
    {
        return bulletLifeDistance;
    }
}
