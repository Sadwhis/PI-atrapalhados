using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using JetBrains.Annotations;

public class CenaMenuController : MonoBehaviour
{
    [SerializeField] Transform[] _painelStart;
    [SerializeField] Transform[] _painelconfig;
    void Start()
    {
        for (int i = 0; i < _painelStart.Length; i++)
        {
          _painelStart[i].localScale = Vector3.zero;
            
        }
        for (int i = 0; i < _painelconfig.Length; i++)
        {
            _painelconfig[i].localScale = Vector3.zero;

        }
        PainelStartCheck(true);
        PainelConfigOff();
    }

    // Update is called once per frame
    public void CenaGame(string Cena)
    {
        SceneManager.LoadScene(Cena);
    }
    IEnumerator TimeStart()
    {
        for(int i = 0; i < _painelStart.Length; i++)
        {
         _painelStart[i].DOScale(1.5f, .25f);
         yield return new WaitForSeconds(0.25f);
         _painelStart[i].DOScale(1, .25f);
        }
    }
    IEnumerator TimeConfig()
    {
        for (int i = 0; i < _painelconfig.Length; i++)
        {
            _painelconfig[i].DOScale(1.5f, .25f);
            yield return new WaitForSeconds(0.25f);
            _painelconfig[i].DOScale(1, .25f);
        }
    }
    public void PainelStartOff()
    {
        for (int i = 0; i < _painelStart.Length; i++)
        {
            _painelStart[i].DOScale(0, .25f);
        }
    }
    public void PainelStartCheck(bool checkON)
    {
        if(checkON == true)
        {
            StartCoroutine(TimeStart());
        }
        else
        {
            PainelStartOff();
        }
    }
    public void PainelConfigCheck(bool checkON)
    {
        if (checkON == true)
        {
            StartCoroutine(TimeConfig());
        }
        else
        {
            PainelConfigOff();
        }
    }
    public void PainelConfigOff()
    {
        for (int i = 0; i < _painelconfig.Length; i++)
        {
            _painelconfig[i].DOScale(0, .25f);
        }
    }
}
