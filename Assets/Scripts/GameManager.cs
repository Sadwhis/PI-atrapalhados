using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int moedas;
    public float _puloMARHS;
    public int _posIni;
    public Transform[] _poStart;
    public Transform _player;


    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _player.position = _poStart[_posIni].position;
        
        //_posIni = PlayerPrefs.GetInt("pos");
    }
    public void ColetarItem(int tipo, int valor)
    {
        if (tipo == 1)
        {
            moedas += valor;
            Debug.Log("Moedas: " + moedas);

            
            if (HudManager.instance != null)
            {
                //HudManager.instance.AnimarTextoPontos(moedas);
            }
        }
    }

    public void Marshmelow()
    {
        


    }
    public void PosStart()
    {

        PlayerPrefs.SetInt("pos", _posIni);
    }


}