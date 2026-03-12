using UnityEngine;
using UnityEngine.UI;

public class RoloFundo : MonoBehaviour
{
    [Header("Velocidade do Rolo")]
    public float _velocidadeX = 0.1f;
    public float _velocidadeY = 0f;

    private RawImage _fundo;

    void Awake()
    {
        // Pega o componente Raw Image automaticamente
        _fundo = GetComponent<RawImage>();
    }

    void Update()
    {
        // Pega as coordenadas atuais do "rolo" (UV)
        Rect uvAtual = _fundo.uvRect;

        // Adiciona o movimento baseado no tempo
        uvAtual.x += _velocidadeX * Time.deltaTime;
        uvAtual.y += _velocidadeY * Time.deltaTime;

        // Aplica de volta na imagem
        _fundo.uvRect = uvAtual;
    }
}