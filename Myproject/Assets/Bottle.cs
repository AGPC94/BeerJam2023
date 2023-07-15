using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] float threeshold;
    [SerializeField] bool isDown;

    void Update()
    {
        if (transform.up.y < threeshold && !isDown)
        {
            Debug.Log("Caido");
            isDown = true;
            Break();
        }
    }

    void Break()
    {

    }
}
