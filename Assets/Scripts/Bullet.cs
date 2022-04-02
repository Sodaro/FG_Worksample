using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
	Rigidbody rb;
	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}
	private void OnTriggerEnter(Collider collider)
	{
        Health healthScript;
        if ((healthScript = collider.gameObject.GetComponent<Health>()) != null)
        {
            healthScript.ReduceHealth(damage);
            gameObject.SetActive(false);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
    }
	private void OnBecameInvisible()
	{
		gameObject.SetActive(false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}

	private void OnCollisionEnter(Collision collision)
	{
		Health healthScript;
		if ((healthScript = collision.gameObject.GetComponent<Health>()) != null)
		{
			healthScript.ReduceHealth(damage);
		}
		gameObject.SetActive(false);
		rb.velocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
	}
}
