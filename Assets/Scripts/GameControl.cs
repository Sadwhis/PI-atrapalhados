using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameControl : MonoBehaviour
{
    public MenuControl _menuControl;
    [SerializeField] Transform _groundBase;
    [SerializeField] float _groundH;
    [SerializeField] float _distance;
    [SerializeField] bool _checkGroundCount;
    public int _groundNumber;
    void Start()
    {
        _groundH = _groundBase.position.y;
        for (int i = 0; i < 10; i++)
        {
           Invoke("GroundStart", 0.25f);
           
            
        }
        
    }

    // Update is called once per frame
    void GroundStart()
    {
        GameObject bullet = GroundPool._groundPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = new Vector2(bullet.transform.position.x, _groundH + _distance); 
            _groundH = bullet.transform.position.y;
            
            //bullet.transform.rotation = turret.transform.rotation;
            bullet.SetActive(true);
        }
    }
    void GroundTime()
    {
        for (int i = 0;i<_groundNumber;i++)
        {
            GroundStart();
            if (i < _groundNumber - 2)
            {
                _checkGroundCount = true;
            }
        }
    }
}
