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


    private float fireCooldown;
    private int currentAmmo;


    private void Start()
    {
        SetWeaponType();
        currentAmmo = magazineSize;
    }

    private void Update()
    {
        fireCooldown += Time.deltaTime;
    }

    public void Shoot(Vector2 direction)
    {
        if (fireCooldown > fireRate)
        {
            fireCooldown = 0f;

            Bullet newBullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            newBullet.Activate(firePoint.position, direction, bulletSpeed, bulletLifeDistance, bulletDamage, bulletIsDestroyable);
        }
    }

    public void SetWeaponType()
    {
        if (weaponType == WeaponType.Pistol)
        {
            magazineSize = 999999;
            fireRate = 0.8f;
            bulletSpeed = 10f;
            bulletLifeDistance = 2f;
            bulletDamage = 1;
            bulletIsDestroyable = true;
        }
        else if (weaponType == WeaponType.Automatic)
        {
            magazineSize = 500;
            fireRate = 0.2f;
            bulletSpeed = 15f;
            bulletLifeDistance = 5f;
            bulletDamage = 1;
            bulletIsDestroyable = true;
        }
        else if (weaponType == WeaponType.Rifle)
        {
            magazineSize = 50;
            fireRate = 1f;
            bulletSpeed = 20f;
            bulletLifeDistance = 10f;
            bulletDamage = 3;
            bulletIsDestroyable = false;
        }
    }

    public bool HasAmmo()
    {
        return currentAmmo > 0;
    }
}
