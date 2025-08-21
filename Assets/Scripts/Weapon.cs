using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] int magazineSize;
    [SerializeField] float fireRate;


    public virtual void Shoot() { }
   
}
