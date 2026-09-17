using UnityEngine;

/// <summary>
/// Da vida ao clima neon: o fundo da camera pulsa entre duas cores,
/// a luz direcional acompanha e tudo esquenta quando o turbo esta ligado.
/// Quando o jogador bate, a tela pisca em vermelho e esfria.
/// </summary>
[RequireComponent(typeof(Camera))]
public class NeonMood : MonoBehaviour
{
    public Color moodA = new Color(0.05f, 0.03f, 0.11f);
    public Color moodB = new Color(0.13f, 0.04f, 0.24f);
    public Color turboMood = new Color(0.02f, 0.13f, 0.18f);
    public Color crashMood = new Color(0.27f, 0.03f, 0.08f);
    public float pulseSpeed = 0.45f;

    private Camera cam;
    private Light sun;
    private Color sunBaseColor;
    private PlayerController player;

    void Start()
    {
        cam = GetComponent<Camera>();

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }

        foreach (Light light in FindObjectsOfType<Light>())
        {
            if (light.type == LightType.Directional)
            {
                sun = light;
                sunBaseColor = light.color;
                break;
            }
        }
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * pulseSpeed * Mathf.PI * 2f) + 1f) * 0.5f;
        Color target = Color.Lerp(moodA, moodB, pulse);

        if (player != null)
        {
            if (player.gameOver)
            {
                target = crashMood;
            }
            else if (player.IsTurboOn())
            {
                target = Color.Lerp(target, turboMood, 0.75f);
            }
        }

        cam.backgroundColor = Color.Lerp(cam.backgroundColor, target, Time.deltaTime * 3f);

        if (sun != null)
        {
            Color sunTarget = sunBaseColor;
            if (player != null && player.IsTurboOn())
            {
                sunTarget = PlayerController.NeonCyan;
            }
            else if (player != null && player.gameOver)
            {
                sunTarget = PlayerController.NeonPink;
            }

            sun.color = Color.Lerp(sun.color, sunTarget, Time.deltaTime * 2.5f);
        }
    }
}
