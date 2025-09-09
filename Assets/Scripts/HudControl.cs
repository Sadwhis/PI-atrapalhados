using DG.Tweening;
using NUnit.Framework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class HudControl : MonoBehaviour
{
    public Slider slider;
    public float speed = 2f;
    [SerializeField] int _lifeStart;
    [SerializeField] int _hitValue;
    [SerializeField] int _life;
    [SerializeField] List<Image> _imagemGameOver;
    GameControl _gameControl;
    void Start()
    {
        slider.maxValue = _lifeStart;
        _life= _lifeStart;
        slider.value = _life;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value =  Mathf.Lerp(slider.value, 0, Time.deltaTime * speed);
    }
    public void HitSlider()
    {
        _life = _life - _hitValue;
        slider.DOValue(_life, 1);
    }
    public void SusLife()
    {
        _life = _life + _hitValue;
        slider.DOValue(_life, speed);
    }
    public void GameOver()
    {
        StartCoroutine(TimeGameOver());
    }
    IEnumerator TimeGameOver()
    {
        _imagemGameOver[0].DOFade(0.8f, .25f);
        yield return new WaitForSeconds(.25f);
        Transform imagem = _imagemGameOver[1].GetComponent<Transform>();
        imagem.DOScale(1.25f, .25f);
        yield return new WaitForSeconds(.25f);
        imagem.DOScale(1.25f, .25f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HitPlayer"))
        {
           _gameControl._hudControl.HitSlider();
        }
    }
}
