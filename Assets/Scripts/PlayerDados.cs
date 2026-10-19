using UnityEngine;

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
        transform.position = InitialPosition.position;
    }
}
