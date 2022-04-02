using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] int pelletCount = 3;
	public override void Fire()
	{
		//base.Fire();
        if (!CanShoot)
            return;
        if (IsReloading)
            return;

        if (currentAmmo <= 0)
        {
            Reload();
            return;
        }

        GameObject[] bullets = ObjectPool.SharedInstance.GetPooledObjects(pelletCount);
        if (bullets.Length > 0)
        {
            for (int i = 0; i < bullets.Length; i++)
			{
                var bullet = bullets[i];
                bullet.transform.position = bulletSpawnPoint.position;
                bullet.transform.rotation = bulletSpawnPoint.rotation;
                float arcAngle = 45;
                float anglePerBullet = arcAngle / bullets.Length;
                bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x, bullet.transform.eulerAngles.y - arcAngle/2 + (anglePerBullet + anglePerBullet * i), bullet.transform.eulerAngles.z); 
                bullet.GetComponent<Bullet>().damage = weaponData.Damage;
                bullet.SetActive(true);
                bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * weaponData.BulletSpeed, ForceMode.Acceleration);
            }
        }

        currentAmmo--;
        shootTimer = weaponData.ShootDelay;
    }
}
