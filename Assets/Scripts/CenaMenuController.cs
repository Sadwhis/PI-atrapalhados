using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using JetBrains.Annotations;

public class CenaMenuController : MonoBehaviour
{
    [SerializeField] Transform[] _painelStart;
    void Start()
    {
        _painelStart[0].DOScale(1, 3);
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
    public void PainelStartOff()
    {
        for (int i = 0; i < _painelStart.Length; i++)
        {
            _painelStart[i].DOScale(0, .25f);
        }
    }
}
