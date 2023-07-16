using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuPresenter : MonoBehaviour
{
    // Start is called before the first frame update
    public Button PlayButton;
    public Button CreditosButton;
    public Button ComoJugarButton;
    public Button SalirButton;
    public Button Vuelta1;
    public Button Vuelta2;
    public GameObject MenuPrincipal;
    public GameObject ComoJuega;
    public GameObject CreditosIm;
    void Start()
    {
        PlayButton.onClick.AddListener(ClickStart);
        CreditosButton.onClick.AddListener(Creditos);
        ComoJugarButton.onClick.AddListener(ComoJugar);
        SalirButton.onClick.AddListener(Salir);
        Vuelta1.onClick.AddListener(Vuelta);
        Vuelta2.onClick.AddListener(Vuelta);

    }
    void ClickStart()
    {
        
        SceneManager.LoadScene("Cocina 2");
        
    }
    void Creditos()
    {
        MenuPrincipal.SetActive(false);
        ComoJuega.SetActive(false);
        CreditosIm.SetActive(true);

    }
    void ComoJugar()
    {
        MenuPrincipal.SetActive(false);
        ComoJuega.SetActive(true);
        CreditosIm.SetActive(false);
    }
    void Salir()
    {
        Application.Quit();
    }
    void Vuelta()
    {
        MenuPrincipal.SetActive(true);
        ComoJuega.SetActive(false);
        CreditosIm.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
