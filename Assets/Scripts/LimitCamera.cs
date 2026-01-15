using UnityEngine;

public class LimitCamera : MonoBehaviour
{

    public GameObject _player;
    void LateUpdate()
    {
        transform.position = new Vector3(_player.transform.position.x, 40 , _player.transform.position.z);
    }

  
}
