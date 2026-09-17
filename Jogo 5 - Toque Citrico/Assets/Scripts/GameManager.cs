using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Cerebro do Toque Citrico: cuida do placar, do combo, das vidas
/// e do ritmo em que as frutas sobem na tela.
/// </summary>
public class GameManager : MonoBehaviour
{
    private const string BestScoreKey = "ToqueCitrico_Recorde";
    private const float BaseSpawnRate = 1.1f;
    private const float MinSpawnRate = 0.3f;
    private const float SpawnRampPerWave = 0.02f;
    private const int MaxLives = 3;
    private const int MaxCombo = 5;

    // Paleta citrica usada no placar.
    private static readonly Color Calmo = new Color(0.9254902f, 1f, 0.6117647f);
    private static readonly Color Quente = new Color(1f, 0.8352941f, 0.30980393f);
    private static readonly Color Pegando = new Color(1f, 0.4392157f, 0.24313726f);

    public List<GameObject> targets;
    public GameObject titleScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public Button restartButton;
    public bool isGameActive;

    private float spawnRate = BaseSpawnRate;
    private int score;
    private int combo;
    private int lives;

    IEnumerator SpawnTarget()
    {
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);

            if (!isGameActive)
            {
                yield break;
            }

            int index = Random.Range(0, targets.Count);
            Instantiate(targets[index]);

            // A cada fruta lancada o jogo acelera um pouquinho.
            spawnRate = Mathf.Max(MinSpawnRate, spawnRate - SpawnRampPerWave);
        }
    }

    /// <summary>Soma pontos; acertos seguidos viram combo e valem mais.</summary>
    public void UpdateScore(int scoreToAdd)
    {
        if (scoreToAdd >= 0)
        {
            combo = Mathf.Min(MaxCombo, combo + 1);
            score += scoreToAdd * combo;
        }
        else
        {
            // Tocou na fruta estragada: perde ponto e zera o combo.
            combo = 0;
            score = Mathf.Max(0, score + scoreToAdd);
        }

        RefreshScoreText();
    }

    /// <summary>Uma fruta boa escapou pela parte de baixo da tela.</summary>
    public void MissedTarget()
    {
        combo = 0;
        lives--;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            RefreshScoreText();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        lives = 0;
        combo = 0;

        int best = PlayerPrefs.GetInt(BestScoreKey, 0);
        if (score > best)
        {
            best = score;
            PlayerPrefs.SetInt(BestScoreKey, best);
            PlayerPrefs.Save();
            gameOverText.text = "ACABOU O SUCO!\nRECORDE NOVO: " + best;
        }
        else
        {
            gameOverText.text = "ACABOU O SUCO!\nRecorde: " + best;
        }

        RefreshScoreText();
        restartButton.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(int difficulty)
    {
        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        score = 0;
        combo = 0;
        lives = MaxLives;
        spawnRate = BaseSpawnRate / difficulty;

        StartCoroutine(SpawnTarget());
        RefreshScoreText();
    }

    private void RefreshScoreText()
    {
        string vidas = new string('*', Mathf.Max(0, lives));
        string linhaCombo = combo > 1 ? "   combo x" + combo : "";
        scoreText.text = "Pontos: " + score + linhaCombo + "\nSucos: " + vidas;

        if (combo >= 4)
        {
            scoreText.color = Pegando;
        }
        else if (combo >= 2)
        {
            scoreText.color = Quente;
        }
        else
        {
            scoreText.color = Calmo;
        }
    }
}
