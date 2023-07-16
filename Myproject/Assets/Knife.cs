using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour, IPooledObject
{
    Rigidbody rb;

    [SerializeField] float forceImpulse;
    [SerializeField] float forceTorque;

    [SerializeField] float forceImpulseMin;
    [SerializeField] float forceImpulseMax;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnObjectSpawn()
    {
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.Euler(Vector3.zero);

        forceImpulse = Random.Range(forceImpulseMin, forceImpulseMax);

        rb.AddForce(transform.forward * forceImpulse, ForceMode.Impulse);
        rb.AddTorque(transform.right * forceTorque, ForceMode.Impulse);
    }
}
