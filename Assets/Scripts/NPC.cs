using Atrapalhados;
using DG.Tweening;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Cursor = UnityEngine.Cursor;

public class NPC : MonoBehaviour
{
    [Header("Configurações")]
    public float tempoAnimacao = 0.5f;
    public float tempoPausa = 0.5f;

    public List<GameObject> _textos = new List<GameObject>();

    private int indiceAtual = 0;

    public bool poClicar = false;

    [SerializeField] bool buttonFase;

    [Header("UI e Referências")]
    public GameObject buttonClicar;
    public GameObject backGroundUI;
    public GameObject buttonMiniJogo;

    private FPController Controller;
    public CinemachineInputAxisController cameraPlayer;
    public Player player;
    private Animator Animator;
    HudManager HudManager;

    private void Start()
    {
        HudManager =
            GameObject.FindWithTag("HudManager")
            .GetComponent<HudManager>();

        Controller =
            GameObject.FindWithTag("Player")
            .GetComponent<FPController>();

        Animator =
            GameObject.FindWithTag("Player")
            .GetComponentInChildren<Animator>();

        foreach (var texto in _textos)
        {
            texto.transform.localScale = Vector3.zero;
            texto.SetActive(false);
        }

        if (backGroundUI != null)
            backGroundUI.SetActive(false);

        if (buttonClicar != null)
            buttonClicar.SetActive(false);

        if (buttonMiniJogo != null)
            buttonMiniJogo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        indiceAtual = 0;

        buttonClicar.SetActive(true);

        if (backGroundUI != null)
        {
            backGroundUI.SetActive(true);
        }

        // Bloqueia movimento e câmera durante o diálogo.
        // NÃO desativa o FPController.
        if (Controller != null)
        {
            Controller.MovementLocked = true;
            Controller.LookLocked = true;
        }

        if (cameraPlayer != null)
        {
            cameraPlayer.enabled = false;
        }

        if (Animator != null)
        {
            if (Animator != null)
            {
                Animator.enabled = true;

                Animator.SetBool("TaAndando", false);
                Animator.SetFloat("VelocidadeAnim", 0f);
            }
        }

        MostrarTextoAtual();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        poClicar = false;

        buttonClicar.SetActive(false);

        if (Controller != null)
        {
            Controller.MovementLocked = false;
            Controller.LookLocked = false;
            Controller._lookSensitivity =
                new Vector2(0.1f, 0.1f);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        EsconderTextoAtual();

        if (cameraPlayer != null)
        {
            cameraPlayer.enabled = true;
        }

        if (backGroundUI != null)
        {
            backGroundUI.transform.DOKill();

            backGroundUI.SetActive(false);

            backGroundUI.transform
                .DOScale(0, tempoAnimacao)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    backGroundUI.SetActive(false);
                });
        }
    }

    public void ProximoDialogo()
    {
        if (!poClicar)
            return;

        poClicar = false;

        EsconderTextoAtual();

        float tempoTotalDeEspera =
            tempoAnimacao + tempoPausa;

        DOVirtual.DelayedCall(
            tempoTotalDeEspera,
            () =>
            {
                indiceAtual++;

                if (indiceAtual < _textos.Count)
                {
                    MostrarTextoAtual();

                    EventSystem.current.SetSelectedGameObject(
                        HudManager._botaoFechar[2]
                    );
                }
                else
                {
                    Debug.Log("Fim do Diálogo!");

                    // Libera o jogador
                    if (Controller != null)
                    {
                        Controller.MovementLocked = false;
                        Controller.LookLocked = false;

                        Controller._lookSensitivity =
                            new Vector2(0.1f, 0.1f);
                    }

                    if (Animator != null)
                    {
                        Animator.enabled = true;
                    }

                    if (cameraPlayer != null)
                    {
                        cameraPlayer.enabled = true;
                    }

                    if (!buttonFase)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.Locked;
                    }

                    buttonClicar.SetActive(false);

                    if (buttonMiniJogo != null)
                    {
                        buttonMiniJogo.SetActive(buttonFase);
                    }

                    if (backGroundUI != null)
                    {
                        backGroundUI.transform.DOKill();

                        buttonMiniJogo.transform.localScale =
                            Vector3.one;

                        backGroundUI.transform
                            .DOScale(0, tempoAnimacao)
                            .SetEase(Ease.InBack)
                            .OnComplete(() =>
                            {
                                Debug.Log("Fim da conversa");

                                backGroundUI.SetActive(false);
                            });
                    }

                    // Caso tenha uma fase para iniciar,
                    // mantém o jogador parado.
                    if (buttonFase)
                    {
                        if (Controller != null)
                        {
                            Controller.MovementLocked = true;
                            Controller.LookLocked = true;
                        }

                        if (Animator != null)
                        {
                            Animator.enabled = false;
                        }

                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                    }

                    EventSystem.current.SetSelectedGameObject(
                        HudManager._botaoFechar[2]
                    );
                }
            }
        );
    }

    private void MostrarTextoAtual()
    {
        if (_textos.Count == 0 ||
            indiceAtual >= _textos.Count)
            return;

        poClicar = false;

        GameObject texto =
            _textos[indiceAtual];

        if (backGroundUI != null)
        {
            backGroundUI.SetActive(true);
            backGroundUI.transform.DOKill();
        }

        texto.SetActive(true);
        texto.transform.DOKill();

        texto.transform.localScale =
            Vector3.zero;

        if (backGroundUI != null)
        {
            backGroundUI.transform
                .DOScale(1, tempoAnimacao)
                .SetEase(Ease.OutBack);
        }

        texto.transform
            .DOScale(1, tempoAnimacao)
            .SetEase(Ease.OutBack)
            .OnComplete(() =>
            {
                poClicar = true;
            });
    }

    private void EsconderTextoAtual()
    {
        if (_textos.Count == 0 ||
            indiceAtual >= _textos.Count)
            return;

        GameObject texto =
            _textos[indiceAtual];

        texto.transform.DOKill();

        texto.transform
            .DOScale(0, tempoAnimacao)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                texto.SetActive(false);
            });
    }

    public void PassarCena(string nomeDaCena)
    {
        SceneManager.LoadScene(nomeDaCena);
    }
}

