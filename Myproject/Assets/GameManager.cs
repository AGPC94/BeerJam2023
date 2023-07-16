using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Vector3 checkPoint;
    public Chef chef;
    public TPController player;

    [SerializeField] string finalScene;

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

    public void Finish()
    {
        //SceneManager.LoadScene(finalScene);
    }
}
