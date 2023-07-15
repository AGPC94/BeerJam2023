using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Vector3 checkPoint;
    [HideInInspector] public Chef chef;
    [HideInInspector] public TPController player;

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        chef = FindObjectOfType<Chef>();
        player = FindObjectOfType<TPController>();

        checkPoint = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCheckpoint(Vector3 cp)
    {
        checkPoint = cp;
    }

    public void Death()
    {

    }

    public void Finish()
    {

    }
}
