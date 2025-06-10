using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameControl : MonoBehaviour
{
    public MenuControl _menuControl;
    [SerializeField] Transform _groundBase;
    [SerializeField] float _groundH;
    [SerializeField] float _distance;
    [SerializeField] bool _checkGroundCount;
    public bool _gameStay;
    public int _groundNumber;
    [SerializeField] Transform _panelSartGame;
    public bool _fimGame;
    [SerializeField] Transform _panelFimGame;
    [SerializeField] Transform _FalaNPC;
    public GameObject _Falando;

    void Start()
    {
        _groundH = _groundBase.position.y;
        Invoke("GroundTime", 0.25f);
        _panelSartGame.gameObject.SetActive(true);
        _panelFimGame.localScale = new Vector3(0,0,0);
    }
    

    // Update is called once per frame
    void GroundStart()
    {
        GameObject bullet = GroundPool._groundPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.GetComponent<GroundPref>()._fimGame.SetActive(false);
            bullet.transform.position = new Vector2(bullet.transform.position.x, _groundH + _distance); 
            _groundH = bullet.transform.position.y;
            if(_checkGroundCount == true)
            {
                bullet.GetComponent<SpriteRenderer>().color = Color.black;
                bullet.GetComponent<GroundPref>()._fimGame.SetActive(true);
            }
            
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
    IEnumerator TimeFimGame()
    {
        yield return new WaitForSeconds(1);
    }
    public void GameStay(bool ativar)
    {
        _gameStay = ativar;
        if(_gameStay == true)
        {
            _panelSartGame.localScale = new Vector3(0, 0, 0);
        }
        else if(_fimGame == true)
        {
            _panelFimGame.localScale = new Vector3(1,1,1);
        }
    }
}
