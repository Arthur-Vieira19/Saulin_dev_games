using UnityEngine;

/// <summary>
/// Empurra o cenario e os obstaculos para a esquerda.
/// Segurar o turbo deixa a pista mais rapida e vale mais pontos.
/// </summary>
public class MoveLeft : MonoBehaviour
{
    private const float BaseSpeed = 12.5f;
    private const float TurboMultiplier = 1.8f;
    private const float LeftBound = -16f;

    private PlayerController player;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!player.gameOver)
        {
            float speed = player.IsTurboOn() ? BaseSpeed * TurboMultiplier : BaseSpeed;
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        if (transform.position.x < LeftBound && gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
