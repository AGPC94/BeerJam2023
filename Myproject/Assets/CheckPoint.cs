using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] bool isUsed;
    [SerializeField] Transform[] wayPoints;
    [SerializeField] Vector3 rotation;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isUsed)
        {
            isUsed = true;
            GameManager.instance.ChangeCheckpoint(transform.position);
            GameManager.instance.chef.ChangeWaypoints(wayPoints, rotation);
        }
    }
}
