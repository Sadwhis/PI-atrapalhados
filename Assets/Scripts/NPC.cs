using Atrapalhados;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Cursor = UnityEngine.Cursor;
using Unity.Cinemachine;

public class NPC : MonoBehaviour
{
    [Header("Configurações")]
    public float tempoAnimacao = 0.5f;
    public float tempoPausa = 0.5f;

    public List<GameObject> _textos = new List<GameObject>();

    private int indiceAtual = 0;
    public bool poClicar = false;

    [Header("UI e Referências")]
    public GameObject buttonClicar;
    public GameObject backGroundUI;

    private GuiaPlayer linhaP;
    private FPController fPController;
    public CinemachineInputAxisController cameraPlayer;


    private void Start()
    {
        
        linhaP = GameObject.FindWithTag("Linha").GetComponent<GuiaPlayer>();
        fPController = GameObject.FindWithTag("Player").GetComponent<FPController>();

        foreach (var texto in _textos)
        {
            texto.transform.localScale = Vector3.zero;
            texto.SetActive(false);
        }
        

        if (backGroundUI != null) backGroundUI.SetActive(false);
        if (buttonClicar != null) buttonClicar.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            indiceAtual = 0;

            buttonClicar.SetActive(true);
            backGroundUI.SetActive(true);

            fPController._lookSensitivity = new Vector2(0, 0);
            backGroundUI.transform.localScale = Vector3.zero;

            MostrarTextoAtual();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            cameraPlayer.enabled = false;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            poClicar = false;
            buttonClicar.SetActive(false);
            fPController._lookSensitivity = new Vector2(0.1f, 0.1f);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            EsconderTextoAtual();

            cameraPlayer.enabled = true;
            backGroundUI.transform.DOKill();
            backGroundUI.transform.DOScale(0, tempoAnimacao).SetEase(Ease.InBack).OnComplete(() =>
            {
                backGroundUI.SetActive(false);
            });
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
            }
            else
            {
                Debug.Log("Fim do Diálogo!");

                if (linhaP != null)
                {
                    linhaP.NpcGeneral();
                }

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                buttonClicar.SetActive(false);

              
                backGroundUI.transform.DOKill();
                backGroundUI.transform.DOScale(0, tempoAnimacao).SetEase(Ease.InBack).OnComplete(() =>
                {
                    backGroundUI.SetActive(false);
                });
            }
        });
    }

    private void MostrarTextoAtual()
    {
        if (_textos.Count == 0 || indiceAtual >= _textos.Count) return;

        poClicar = false;
        GameObject texto = _textos[indiceAtual];

        backGroundUI.SetActive(true);
        texto.SetActive(true);

        backGroundUI.transform.DOKill();
        texto.transform.DOKill();

        texto.transform.localScale = Vector3.zero;

        backGroundUI.transform.DOScale(1, tempoAnimacao).SetEase(Ease.OutBack);

        
        texto.transform.DOScale(1, tempoAnimacao).SetEase(Ease.OutBack).OnComplete(() =>
        {
            poClicar = true;
        });
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