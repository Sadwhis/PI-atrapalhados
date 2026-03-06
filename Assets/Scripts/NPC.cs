using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;


public class NPC : MonoBehaviour
{
    [Header("Configurações")]
    public float tempoAnimacao = 0.5f;
    public float tempoPausa = 3f;
    public List<GameObject> _textos = new List<GameObject>();

    private int indiceAtual = 0;
    private bool poClicar = true;

    public GameObject buttonClicar;
    public GameObject backGroundUI;

    private void Start()
    {
        foreach (var texto in _textos)
        {
            texto.transform.localScale = Vector3.zero;
            texto.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indiceAtual = 0;
            poClicar = true;
            buttonClicar.SetActive(true);
            backGroundUI.SetActive(true);
            MostrarTextoAtual();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EsconderTextoAtual();
            
            buttonClicar.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            backGroundUI.transform.DOKill();
            backGroundUI.transform.DOScale(0, tempoAnimacao).SetEase(Ease.InBack);
        }
    }

    public void ProximoDialogo()
    {
        
        if (!poClicar) return;

        poClicar = false; 
        EsconderTextoAtual();

       
        float tempoTotalDeEspera = tempoAnimacao + tempoPausa;

        
        DOVirtual.DelayedCall(tempoTotalDeEspera, () =>
        {
            indiceAtual++;

            if (indiceAtual < _textos.Count)
            {
                MostrarTextoAtual();
                poClicar = true; 
            }
            else
            {
                Debug.Log("Fim do didi!");

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                backGroundUI.SetActive(false);
                buttonClicar.SetActive(false);

                backGroundUI.transform.DOKill();
                backGroundUI.transform.DOScale(0, tempoAnimacao).SetEase(Ease.InBack);

            }
        });
    }

    private void MostrarTextoAtual()
    {
        if (_textos.Count == 0 || indiceAtual >= _textos.Count) return;

        GameObject texto = _textos[indiceAtual];
        backGroundUI.SetActive (true);
        texto.SetActive(true);
        backGroundUI.transform.DOKill();
        texto.transform.DOKill();
        //backGroundUI.transform.localScale = Vector3.zero;
        texto.transform.localScale = Vector3.zero;
        backGroundUI.transform.DOScale(1, tempoAnimacao).SetEase(Ease.OutBack);
        texto.transform.DOScale(1, tempoAnimacao).SetEase(Ease.OutBack);
    }

    private void EsconderTextoAtual()
    {
        if (_textos.Count == 0 || indiceAtual >= _textos.Count) return;

        GameObject texto = _textos[indiceAtual];
        texto.transform.DOKill();
        texto.transform.DOScale(0, tempoAnimacao).SetEase(Ease.InBack).OnComplete(() =>
        {
            texto.SetActive(false);
        });
       
    }
}