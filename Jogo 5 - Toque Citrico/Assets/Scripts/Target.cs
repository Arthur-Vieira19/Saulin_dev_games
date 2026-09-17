using UnityEngine;

/// <summary>
/// Fruta (ou fruta estragada) que sobe da parte de baixo da tela.
/// Ganha cor, tamanho e giro aleatorios pra nunca subir igual.
/// </summary>
public class Target : MonoBehaviour
{
    private const float MinSpeed = 13f;
    private const float MaxSpeed = 18f;
    private const float MaxTorque = 14f;
    private const float XRange = 4.5f;
    private const float YSpawnPos = -2.5f;

    // Tons citricos sorteados pra cada fruta boa.
    private static readonly Color[] CitrusTints =
    {
        new Color(1f, 0.85f, 0.35f),
        new Color(0.75f, 0.94f, 0.4f),
        new Color(1f, 0.6f, 0.28f),
        new Color(0.6f, 0.95f, 0.78f)
    };

    private Rigidbody targetRb;
    private GameManager gameManager;
    private Color tint;

    public int pointValue;
    public ParticleSystem explosionParticle;

    void Start()
    {
        targetRb = GetComponent<Rigidbody>();

        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        transform.position = RandomSpawnPos();
        transform.localScale *= Random.Range(0.85f, 1.15f);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Paint();
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(MinSpeed, MaxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-MaxTorque, MaxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-XRange, XRange), YSpawnPos);
    }

    /// <summary>Pinta a fruta: tons citricos nas boas, roxo acinzentado na estragada.</summary>
    void Paint()
    {
        tint = gameObject.CompareTag("Bad")
            ? new Color(0.45f, 0.35f, 0.55f)
            : CitrusTints[Random.Range(0, CitrusTints.Length)];

        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = tint;
        }
    }

    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            ParticleSystem burst = Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

            // O estouro sai na mesma cor da fruta que foi tocada.
            ParticleSystem.MainModule main = burst.main;
            main.startColor = tint;

            gameManager.UpdateScore(pointValue);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        // Deixar a fruta boa cair custa um suco, mas o jogo continua.
        if (!gameObject.CompareTag("Bad"))
        {
            gameManager.MissedTarget();
        }
    }
}
