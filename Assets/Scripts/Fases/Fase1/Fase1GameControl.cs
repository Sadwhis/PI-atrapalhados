using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class Fase1GameControl : MonoBehaviour
{
    [SerializeField] Transform _groundBase;
    [SerializeField] float _groundH;
    [SerializeField] float _distance;
    [SerializeField] bool _checkGroundCount;

    public int _groundNumber;
    public bool _fimGame;
    public Fase1HudControl _fase1HudControl;
    public Fase1MoverPlayer _fase1MoverPlayer;
    void Start()
    {
        _fase1MoverPlayer = GameObject.FindWithTag("Player").GetComponent<Fase1MoverPlayer>();
        _fase1HudControl = GameObject.FindWithTag("Hud").GetComponent<Fase1HudControl>();
        _groundH = _groundBase.position.y;
        Invoke("GroundTime", 0.25f);
        
    }

      void GroundTime()
      {
        for (int i = 0;i<_groundNumber;i++)
        {
            GroundStart();
            if (i == _groundNumber-2)
            {
              _checkGroundCount = true;
            }
        }
      }


    // Update is called once per frame
    void GroundStart()
    {
        GameObject bullet = GroundPool._groundPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.GetComponent<GroundPref>()._fimGame.SetActive(false);
            
           
            if(_checkGroundCount == true)
            {
                bullet.GetComponent<SpriteRenderer>().color = Color.black;
                bullet.GetComponent<GroundPref>()._fimGame.SetActive(true);
            }
            
            bullet.transform.position = new Vector2(bullet.transform.position.x, _groundH + _distance); 
            _groundH = bullet.transform.position.y;
            //bullet.transform.rotation = turret.transform.rotation;
           
            bullet.SetActive(true);
        }
    }
  
    IEnumerator TimeFimGame()
    {
        yield return new WaitForSeconds(1);
    }
 
    public void ResetarCena()
    {
        SceneManager.LoadScene("Fase1");
    }
    public void ResetarCena(string Cena)
    {
        SceneManager.LoadScene(Cena);
    }
}
