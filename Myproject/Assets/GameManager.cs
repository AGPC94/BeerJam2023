using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Vector3 checkPoint;
    [HideInInspector] public Chef chef;
    [HideInInspector] public TPController player;

    [SerializeField] string finalScene;

    public static GameManager instance;

    bool isCursorLocked;

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

        isCursorLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isCursorLocked = !isCursorLocked;

            if (isCursorLocked)
            {
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = false;
            }
            else
            {
                //Cursor.lockState = CursorLockMode.None;
                ///Cursor.visible = true;
            }
        }
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
