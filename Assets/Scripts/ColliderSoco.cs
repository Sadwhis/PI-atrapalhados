using Atrapalhados;
using UnityEngine;

public class ColliderSoco : MonoBehaviour
{
    [HideInInspector] public FlyEnemy inimigoNoAlcance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            inimigoNoAlcance = other.GetComponent<FlyEnemy>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            inimigoNoAlcance = null;
        }
    }
}