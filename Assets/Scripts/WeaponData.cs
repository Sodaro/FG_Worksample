using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Weapon Data", order = 51)]
public class WeaponData : ScriptableObject
{
	[SerializeField]
	int ammoCapacity;
	[SerializeField]
	int damage;
	[SerializeField]
	float shootDelay;

	[SerializeField]
	float reloadDuration;

	[SerializeField]
	float throwDamage;

	[SerializeField]
	float warmupTime; //miniguns

	[SerializeField]
	float range;

	[SerializeField]
	float bulletSpeed;

	public int AmmoCapacity => ammoCapacity;
	public int Damage => damage;
	public float ShootDelay => shootDelay;
	public float ReloadDuration => reloadDuration;
	public float Range => range;
	public float BulletSpeed => bulletSpeed;
	public float ThrowDamage => throwDamage;
	public float WarmupTime => warmupTime;
}
