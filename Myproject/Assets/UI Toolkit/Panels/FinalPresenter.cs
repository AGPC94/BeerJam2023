using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalPresenter : MonoBehaviour
{
    // Start is called before the first frame update
    public Button PlayButton;

    void Start()
    {
        PlayButton.onClick.AddListener(ClickStart);


    }
    void ClickStart()
    {

        SceneManager.LoadScene("Menu_Principal");

        // Update is called once per frame
        void Update()
        {

        }
    }
}
