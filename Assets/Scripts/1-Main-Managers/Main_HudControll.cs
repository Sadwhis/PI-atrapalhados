using UnityEngine;
using DG.Tweening;

public class Main_HudControll : MonoBehaviour
{
    PlayerController _moveScript;
    [SerializeField] GameObject _painelStart;
    public GameObject _DialogoIni;

    void Start()
    {
        _moveScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        _DialogoIni.SetActive(false);
    }

    public void ButaoStart()
    {
        _moveScript.rb.bodyType = RigidbodyType2D.Dynamic;
        _painelStart.SetActive(false);
        _moveScript.velocidade = 5f;
    }

    public void ButaoExitIni()
    {

        _DialogoIni.transform.DOScale(Vector3.zero, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _DialogoIni.SetActive(false);
        });

        _moveScript.rb.linearVelocity = Vector2.zero; 
        _moveScript.rb.bodyType = RigidbodyType2D.Dynamic;
        _moveScript.velocidade = 5f;
    }

    public void MostrarDialogo()
    {
        _DialogoIni.SetActive(true);
        _DialogoIni.transform.localScale = Vector3.zero;
        _DialogoIni.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

       
        _moveScript.rb.linearVelocity = Vector2.zero;
        _moveScript.rb.bodyType = RigidbodyType2D.Kinematic;
        _moveScript.velocidade = 0f;
    }
}
