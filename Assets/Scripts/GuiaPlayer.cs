using UnityEngine;

public class GuiaPlayer : MonoBehaviour
{
    [SerializeField] Transform npcIni;
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    
    void Update()
    {
        NpcIni();
    }
    void NpcIni()
    {
        lineRenderer.SetPosition(0,transform.position);
        lineRenderer.SetPosition(1, npcIni.position);
    }
}
