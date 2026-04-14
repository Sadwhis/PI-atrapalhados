using UnityEngine;

public class ClipControl : MonoBehaviour
{
    AudioSource audioSource;
    void Start()
    {
        // audioSource = GetComponent<AudioSource>();  
    }


    public void Desativar()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("TimeDesativar", audioSource.clip.length);
    }

    public void TimeDesativar()
    {

        gameObject.SetActive(false);
    }
}
