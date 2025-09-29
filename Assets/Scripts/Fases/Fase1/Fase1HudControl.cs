using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fase1HudControl : MonoBehaviour
{
    Fase1GameControl _fase1GameControl;
    [SerializeField] string[] _nomeCores;
    [SerializeField] TextMeshProUGUI _textoComando;
    public Transform _painelStart;
    public Transform _painelEnd;

    void Start()
    {
       _fase1GameControl = GameObject.FindWithTag("GameController").GetComponent<Fase1GameControl>();
        CorPulo(0);

        
        _painelStart.localScale = Vector3.zero;


        MostrarStartPanel();
    }

    void MostrarStartPanel()
    {
        _painelStart.gameObject.SetActive(true);
        _painelStart.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack); 
    }

    public void MostrarEndPanel()
    {
        _painelEnd.gameObject.SetActive(true);
        _painelEnd.localScale = Vector3.zero;
        _painelEnd.DOScale(Vector3.one, 1f).SetEase(Ease.OutBack);
    }

    public void BotaoStart()
    {
        _painelStart.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _painelStart.gameObject.SetActive(false);
        });

        _fase1GameControl._fase1MoverPlayer.PlayerLiberar();
    }

    public void CorPulo(int number)
    {
        _textoComando.text = _nomeCores[number];
    }

    public void ResetarCena()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
