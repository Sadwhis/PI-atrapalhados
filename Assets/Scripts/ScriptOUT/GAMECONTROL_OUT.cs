using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GAMECONTROL_OUT : MonoBehaviour
{

    [SerializeField] public GameObject _Falando;
    [SerializeField] public bool _BD;
    [SerializeField] public bool _BDE;


    void Start()
    {
        


    }

    
    void Update()
    {
        
    }

    

    public void ButtonDialogue(bool ativar)
    {
        _BD = ativar;
        if (_BD == true)
        {
            SceneManager.LoadScene("Sapo-Cururu");

        }

    }

    public void ButtonExit(bool ativarE)
    {

        _BDE = ativarE;
        if (_BDE == true)
        {
           _Falando.SetActive(false);

        }


    }
}
