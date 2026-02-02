using UnityEngine;

public class InimigoDoce : MonoBehaviour
{
    [SerializeField] float _velocidade = 5f;
    private int _direcao = 1; 

    void Update()
    {
        
        transform.Translate(Vector3.right * _direcao * _velocidade * Time.deltaTime);

        
        if (Mathf.Abs(transform.position.x) > 15f)
        {
            Destroy(gameObject);
        }
    }

    
    public void Configurar(bool indoParaDireita)
    {
        _direcao = indoParaDireita ? 1 : -1;
        
        if (!indoParaDireita) transform.localScale = new Vector3(-1, 1, 1);
    }

  
}