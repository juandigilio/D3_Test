using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int magazineSize;
    [SerializeField] float fireRate;


    public abstract void Shoot();
   
}
