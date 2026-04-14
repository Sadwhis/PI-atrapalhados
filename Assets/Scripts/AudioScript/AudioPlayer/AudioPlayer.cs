using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] AudioPlayerAsset somIni;

    public static AudioPlayer Instance;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
        Invoke("SomStart", 1);
    }
    public void SomStart(int valueSom)
    {
        GameObject somObj = ObjectPool.SharedInstance.GetPooledObject();

        if (somObj != null)
        {
            somObj.SetActive(true);
          

            AudioSource source = somObj.GetComponent<AudioSource>();

            switch (valueSom)
            {
                case 0: source.clip = somIni.explo; break;
                case 1: source.clip = somIni.hit; break;
                case 2: source.clip = somIni.morte; break;
            }

            source.Play();
            somObj.GetComponent<ClipControl>().Desativar();
        }
    }

}
