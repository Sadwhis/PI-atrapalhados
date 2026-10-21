using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDados : MonoBehaviour
{
    [SerializeField] private Transform InitialPosition;
    void Start()
    {
        Respawn();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Respawn()
    {
       float x = PlayerPrefs.GetFloat("checkpointX");
       float y = PlayerPrefs.GetFloat("checkpointY");
        Vector2 checkpointPosition = new Vector2(x, y);
        if(checkpointPosition != Vector2.zero)
        {
            transform.position = checkpointPosition;
        }
        else
        {
            transform.position = InitialPosition.position;
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("CheckPoint"))
        {
            Vector2 position = collision.transform.position;
            PlayerPrefs.SetFloat("checkpointX", position.x);
            PlayerPrefs.SetFloat("checkpointY", position.y);
        }
    }
}
