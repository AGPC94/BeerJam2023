using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        ObjectPooler.instance.SpawnFromPool("Cube", transform.position, Quaternion.identity);
    }
}
