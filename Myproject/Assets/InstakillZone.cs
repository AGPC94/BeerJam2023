using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstakillZone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Instakill");
            other.GetComponent<TPController>().Die();
        }
    }
}
