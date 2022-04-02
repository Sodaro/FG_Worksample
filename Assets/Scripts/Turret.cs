using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    enum TurretState { SEARCHING, ATTACKING};
    [SerializeField] TurretState currentState = TurretState.SEARCHING;
    [SerializeField] Weapon weapon;
    [SerializeField] GameObject target;
    [SerializeField] Health health;
    [SerializeField] Transform weaponPos;
    [SerializeField] GameObject weaponPrefab;
	private void Awake()
	{
        GameObject weaponObject = Instantiate(weaponPrefab);
        weapon = weaponObject.GetComponentInChildren<Weapon>();
        health = GetComponent<Health>();
        weapon.PickUp(weaponPos);
	}
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health.CurrentHealth <= 0)
		{
            Death();
            return;
		}

        if (currentState == TurretState.SEARCHING)
		{
            int layerMask = 1 << 8;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 35f, layerMask);
            if (colliders.Length > 0)
			{
                if (CanSeeTarget(colliders[0].transform, 1))
				{
                    target = colliders[0].gameObject;
                    currentState = TurretState.ATTACKING;
                }

            }
        }
        else if (currentState == TurretState.ATTACKING)
		{
            if (weapon.Ammo <= 0)
			{
                weapon.Reload();
                return;
            }
            if (Vector3.Distance(transform.position, target.transform.position) > 35f || !CanSeeTarget(target.transform, 1))
			{
                currentState = TurretState.SEARCHING;
                return;
			}
            transform.rotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
            weapon.Fire();
		}
    }

    bool CanSeeTarget(Transform targetTransform, int collisionLayerMask)
    {
        return !Physics.Linecast(transform.position, targetTransform.position, out RaycastHit _, collisionLayerMask);
    }

    public void Death() //called with health SendMessage
	{
        weapon.Drop();
        Destroy(gameObject);
	}
}
