using TMPro;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    GameControl _gameControl;
    [SerializeField] string[] _nomeCores;
    [SerializeField] TextMeshProUGUI _textoComando;
    void Start()
    {
        _gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        CorPulo(0);
    }

    public void CorPulo(int number)
    {
        // if (number == 0)
        //{
        _textoComando.text = _nomeCores[number];
        // }
    }
    //Texto texte pra ver se o junijho volta pra gente
    
}
