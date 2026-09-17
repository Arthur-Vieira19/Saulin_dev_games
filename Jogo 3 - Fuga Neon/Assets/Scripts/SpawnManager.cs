using UnityEngine;

/// <summary>
/// Solta obstaculos na pista. O intervalo entre eles encurta
/// conforme o jogador sobrevive, entao a corrida vai apertando.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    public GameObject[] obstaclePrefabs;

    private readonly Vector3 spawnPos = new Vector3(26, 1, 0);
    private const float StartDelay = 2f;
    private const float StartInterval = 2.1f;
    private const float MinInterval = 0.95f;
    private const float RampPerSpawn = 0.045f;

    private float interval = StartInterval;
    private float timer;
    private PlayerController player;

    void Start()
    {
        timer = -StartDelay;
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player.gameOver)
        {
            return;
        }

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            SpawnObstacle();
            interval = Mathf.Max(MinInterval, interval - RampPerSpawn);
        }
    }

    void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0)
        {
            return;
        }

        int index = Random.Range(0, obstaclePrefabs.Length);
        GameObject obstacle = Instantiate(obstaclePrefabs[index], spawnPos, obstaclePrefabs[index].transform.rotation);

        // Cada obstaculo ganha um tamanho um pouco diferente, so pra variar a silhueta.
        obstacle.transform.localScale *= Random.Range(0.85f, 1.2f);
    }
}
