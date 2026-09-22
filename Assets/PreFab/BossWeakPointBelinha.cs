using Atrapalhados;
using UnityEngine;

/// <summary>
/// Coloque este script num GameObject filho, posicionado em cima do FlyEnemy,
/// com um Collider marcado como "Is Trigger".
/// Quando o player pisa nele (vindo de cima), desconta uma vida do inimigo.
/// </summary>
[RequireComponent(typeof(Collider))]
public class BossWeakPoint : MonoBehaviour
{
    [Tooltip("Referência pro script do inimigo voador (o corpo dele).")]
    [SerializeField] private FlyEnemy _flyEnemy;

    [Tooltip("Layer do player, pra só reagir quando ele encostar.")]
    [SerializeField] private LayerMask _playerLayer;

    [Tooltip("Só conta o pulo se o player estiver caindo (velocidade Y menor que isso). " +
             "Evita contar hit se o player encostar de lado ou vindo de baixo.")]
    [SerializeField] private float _maxUpwardVelocityToCount = 0.5f;

    [SerializeField] private float _bounceHeight = 3f;
    private void Reset()
    {
        // Tenta achar o FlyEnemy automaticamente no pai, pra facilitar no editor.
        _flyEnemy = GetComponentInParent<FlyEnemy>();

        // Garante que o collider já vem configurado como trigger.
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Confere se quem entrou é o player (por layer).
        if (((1 << other.gameObject.layer) & _playerLayer) == 0)
            return;

        // Confere se o player está vindo de cima (pulando/caindo em cima),
        // e não colidindo de lado.
        Rigidbody playerRb = other.attachedRigidbody;
        if (playerRb != null && playerRb.linearVelocity.y > _maxUpwardVelocityToCount)
            return;
        FPController player = other.GetComponent<FPController>();
        if (player != null)
            player.Bounce(_bounceHeight);

      
        if (_flyEnemy == null)
        {
            Debug.LogWarning("BossWeakPoint sem referência pro FlyEnemy!", this);
            return;
        }

        _flyEnemy.TakeDamage();

        // Opcional: dar um pequeno pulo/impulso no player quando ele acerta o ponto fraco,
        // tipo Mario pisando em inimigo. Se quiser isso, descomente e ajuste:
        //
        // if (playerRb != null)
        // {
        //     Vector3 vel = playerRb.linearVelocity;
        //     vel.y = 6f; // força do "pulinho" de bounce
        //     playerRb.linearVelocity = vel;
        // }
    }
}