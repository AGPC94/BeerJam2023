using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlatformMoving : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [Space]
    [SerializeField] bool isActive = true;
    [SerializeField] bool isLooping = true;
    [SerializeField] float speed = 10;
    [SerializeField] float waitTime = 0.5f;
    [SerializeField] float maxDistance = 0;

    bool isWaiting;
    float distance;
    int current;

    void Start()
    {
        current = 0;
    }


    void FixedUpdate()
    {
        if (!isWaiting && isActive)
        {
            Move();
        }
    }
    void NextWayPoint()
    {
        isWaiting = false;

        current++;

        if (current >= waypoints.Length)
        {
            if (isLooping)
            {
                current = 0;
            }
            else
            {
                current = waypoints.Length;
                isWaiting = true;
            }
        }

    }

    void Move()
    {
        Vector3 wp = waypoints[current].transform.position;

        transform.position = Vector3.MoveTowards(transform.position, wp, speed * Time.deltaTime);

        distance = Mathf.Abs(Vector3.Distance(transform.position, wp));

        if (distance <= maxDistance)
        {
            isWaiting = true;
            Invoke("NextWayPoint", waitTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
            isActive = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    void OnDrawGizmos()
    {
        if (waypoints == null)
            return;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (i != waypoints.Length - 1)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            else
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
        }

    }
}