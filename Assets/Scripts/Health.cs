using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	[SerializeField]
	private float totalHealth;

	[SerializeField] GameEvent damageTaken;
	public GameEvent healthDepleted;

	[SerializeField]
	private float currentHealth = 0;

	public float CurrentHealth => currentHealth;

	private void Start()
	{
		if (currentHealth == 0)
			currentHealth = totalHealth;
	}
	//public void FullHeal()
	//{
	//	currentHealth = totalHealth;
	//}
	public void ReduceHealth(float amount)
	{
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			healthDepleted?.Invoke();
		}
	}
			
}
