using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    private List<GameObject> pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    [SerializeField] bool shouldExpand = true;


	private void Awake()
	{
        SharedInstance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for (int i = 0; i < amountToPool; i++)
		{
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
		}
    }

    public GameObject GetPooledObject()
	{
        for (int i = 0; i < amountToPool; i++)
		{
            if (!pooledObjects[i].activeInHierarchy)
                return pooledObjects[i];
		}
        if (shouldExpand)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }
        else
        {
            return null;
        }
    }

    public GameObject[] GetPooledObjects(int count)
	{
        List<GameObject> objects = new List<GameObject>();

        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy && objects.Count < count)
                objects.Add(pooledObjects[i]);
        }

        if (shouldExpand)
        {
            while (objects.Count < count)
			{
                GameObject obj = Instantiate(objectToPool);
                obj.SetActive(false);
                pooledObjects.Add(obj);
                objects.Add(obj);
            }
        }
        return objects.ToArray();
    }
}
