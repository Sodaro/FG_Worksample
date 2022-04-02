using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] GameObject target;
    Health targetHealthScript;
	private void Awake()
	{
        
	}
	// Start is called before the first frame update
	void Start()
    {
        targetHealthScript = target.GetComponentInChildren<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNewTarget()
	{
        //targetHealthScript.SetHealthTotal()
        //targetHealthScript.FullHeal();
        target.transform.position = new Vector3(transform.position.x + Random.Range(-10, 11), target.transform.position.y, transform.position.z - Random.Range(-10, 11));
	}
}
