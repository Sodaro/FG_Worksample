using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: ADD PICKUP FOR WEAPONS, POSITION WEAPON IN CORRECT POSITION

public class Weapon : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] protected WeaponData weaponData;
    protected float shootTimer = 0;
    protected int currentAmmo = 0;
    protected float reloadTimer = 0f;
    [SerializeField] protected float warmupTimer;
    [SerializeField] protected Transform bulletSpawnPoint;

    public bool isHeld = false;
    protected bool CanShoot => shootTimer <= 0;
    protected bool IsReloading => reloadTimer > 0;
    protected bool IsWarmingUp => warmupTimer > 0;

    public WeaponData Data => weaponData;
    public int Ammo => currentAmmo;
    public void Stop() => isShooting = false;

    bool isShooting = false;

	private void Awake()
	{
        rb = GetComponentInParent<Rigidbody>();
        currentAmmo = weaponData.AmmoCapacity;
        warmupTimer = weaponData.WarmupTime;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isHeld)
            return;
        if (!CanShoot)
            shootTimer -= Time.deltaTime;
        if (IsReloading)
            reloadTimer -= Time.deltaTime;
    }

	private void LateUpdate()
	{
        if (!isShooting)
		{
            warmupTimer += Time.deltaTime;
            warmupTimer = Mathf.Clamp(warmupTimer, 0, weaponData.WarmupTime);
        }
	}

	public virtual void Fire()
    {
        if (!CanShoot)
            return;
        if (IsReloading)
            return;
        if (currentAmmo <= 0)
            return;

        isShooting = true;
        if (IsWarmingUp)
		{
            warmupTimer -= Time.deltaTime;
            return;
		}

        GameObject bulletInstance = ObjectPool.SharedInstance.GetPooledObject();
        if (bulletInstance)
		{
            bulletInstance.transform.SetPositionAndRotation(bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            bulletInstance.GetComponent<Bullet>().damage = weaponData.Damage;
            bulletInstance.SetActive(true);
            bulletInstance.GetComponent<Rigidbody>().AddForce(bulletInstance.transform.forward * weaponData.BulletSpeed, ForceMode.Acceleration);

        }

        currentAmmo--;
        shootTimer = weaponData.ShootDelay;
    }

    public void PickUp(Transform parentObject) //disable gravity, apply constraints, parent object to transform parameter
	{
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.parent.parent = parentObject;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.useGravity = false;
        transform.parent.position = parentObject.position;
        transform.parent.rotation = parentObject.rotation;
        GetComponentInParent<MeshCollider>().enabled = false;
        isHeld = true;
    }

    public void Reload()
    {
		currentAmmo = weaponData.AmmoCapacity;
		reloadTimer = weaponData.ReloadDuration;
	}

    public void Throw() //throw weapon and apply rotational force
	{
        GetComponentInParent<MeshCollider>().enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
		transform.parent.parent = null;

        isHeld = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.AddForce(transform.forward * 500f, ForceMode.Impulse);
        rb.AddTorque(transform.up * 1000f);
    }

    public void Drop() //enable rigidbody gravity and remove constraints
	{
        GetComponentInParent<MeshCollider>().enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        transform.parent.parent = null;

        isHeld = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

	private void OnTriggerEnter(Collider other) //thrown weapon as projectile
	{
		if (rb.velocity.magnitude > 10)
		{
            Health healthScript;
            if ((healthScript = other.gameObject.GetComponent<Health>()) != null)
            {
                healthScript.ReduceHealth(weaponData.ThrowDamage);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
        }
	}
}
