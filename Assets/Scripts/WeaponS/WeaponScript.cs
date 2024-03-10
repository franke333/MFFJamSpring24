using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponScript : ScriptableObject
{
    public Sprite sprite;
    public BulletScript bulletPrefab;

    public int ammoPerPack = 10;
    public int costPerPack = 10;

    public int currentAmmo;
    public float shootCooldown = 0.5f;
    public float currentCooldown = 0f;

    public float shakeMultiplier = 1f;

    public float accuracy = 1f;
    public float bulletSpeed = 10f;

    public bool Shoot(Transform shootPoint)
    {
        if (currentCooldown < 0 && currentAmmo > 0)
        {

            var bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            currentAmmo--;

            float angle = Random.Range(0, 2 * Mathf.PI);
            Vector3 spread = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
            float acc = Random.Range(0, 1 - accuracy);
            var direction = (shootPoint.forward + spread * acc).normalized;

            bullet.speed = bulletSpeed;
            bullet.direction = direction;
            AudioManager.Instance.shootSource.Play();
            currentCooldown = shootCooldown;
            return true;
        }
        else if (currentAmmo <= 0 && currentCooldown < 0)
        {
            currentCooldown = shootCooldown;
            AudioManager.Instance.emptyGunSource.Play();
        }
        
        return false;
        
    }

}
