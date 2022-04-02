using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 offset;
    [SerializeField] float catchUpSpeed = 5f;

    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        if (Vector3.Distance(target.transform.position + offset, transform.position) > 1)
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position + offset, catchUpSpeed * Time.deltaTime);
    }
}
