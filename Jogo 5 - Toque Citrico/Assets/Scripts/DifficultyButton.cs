using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Botao da tela de titulo. Alem de escolher a dificuldade,
/// combina a cor do proprio botao com a intensidade escolhida.
/// </summary>
public class DifficultyButton : MonoBehaviour
{
    private static readonly Color[] Intensidades =
    {
        new Color(0.76f, 0.94f, 0.35f), // suave
        new Color(1f, 0.84f, 0.31f),    // medio
        new Color(1f, 0.44f, 0.24f)     // acido
    };

    private Button button;
    private GameManager gameManager;

    public int difficulty;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(SetDifficulty);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Color cor = Intensidades[Mathf.Clamp(difficulty - 1, 0, Intensidades.Length - 1)];
        ColorBlock cores = button.colors;
        cores.normalColor = cor;
        cores.highlightedColor = Color.Lerp(cor, Color.white, 0.35f);
        cores.pressedColor = cor * 0.72f;
        cores.selectedColor = cores.highlightedColor;
        button.colors = cores;
    }

    void SetDifficulty()
    {
        gameManager.StartGame(difficulty);
    }
}
