using UnityEngine;

public class GuiaPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform npcIni;
    [SerializeField] Transform general;
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        NpcIni();
    }

    
    void Update()
    {
        LinhaAtual();
    }
    void LinhaAtual()
    {
        lineRenderer.SetPosition(0, player.position);
       
    }
    void NpcIni()
    {
        
        lineRenderer.SetPosition(1, npcIni.position);
    }

    public void NpcGeneral() 
    {
        lineRenderer.SetPosition(1, general.position);
    }
}
