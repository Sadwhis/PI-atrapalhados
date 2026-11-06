using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UiControll : MonoBehaviour
{
    //public CanvasGroup CanvasGroup;
    //public float duration;
    //public Button Jogar;
    //public Button DarNoPé;
    [SerializeField] Transform Titulo;
    void Start()
    {
        // CanvasGroup.DOFade(1f, duration);
        //Jogar.onClick.AddListener(PlayButtonAnimation);
        //DarNoPé.onClick.AddListener(DarNoPéButtonAnimation);
        Titulo.transform.DOShakeScale(1, new Vector3(1, 0.5f, 0), 5);
    }

    // Update is called once per frame
    void Update()
    {

    }
    //void PlayButtonAnimation()
    //{
    //.transform.DOShakeScale(Vector3.one * 0.3f, 10, 1f);
    //}
    // DarNoPéButtonAnimation()
    //

    //}

}

