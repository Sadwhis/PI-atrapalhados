using UnityEngine;

public class InimigoMove : MonoBehaviour
{
    public float speed = 2f;
    public float distance = 1f;
    private bool isRight = true;

    public Transform groundCheck;

    void Update()
    {
        // Movimento para frente
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Raycast para verificar se tem chão
        RaycastHit2D ground = Physics2D.Raycast(groundCheck.position, Vector2.down, distance);

        if (ground.collider == null) // se não encontrar chão
        {
            if (isRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                isRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isRight = true;
            }
        }
    }

    // Visualizar o raycast na cena (opcional)
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * distance);
        }
    }
}
