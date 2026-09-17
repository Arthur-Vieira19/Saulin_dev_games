using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla o corredor: pulo duplo, turbo, colisao e o HUD neon.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private const string BestScoreKey = "FugaNeon_Recorde";

    // Paleta neon do jogo (tambem usada pelo NeonMood).
    public static readonly Color NeonPink = new Color(1f, 0.29f, 0.76f);
    public static readonly Color NeonCyan = new Color(0.36f, 0.95f, 1f);
    public static readonly Color NeonLime = new Color(0.72f, 1f, 0.35f);

    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    private bool doubleJump;
    private bool turboOn;
    private float bestScore;
    private GUIStyle hudStyle;
    private GUIStyle bigStyle;

    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce = 2;
    public float gravityModifier;
    public bool isOnGround = true;
    public bool gameOver = false;
    public float score = 0;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();

        // Gravidade absoluta: evita que o valor se acumule a cada reinicio da fase.
        Physics.gravity = new Vector3(0, -9.81f * gravityModifier, 0);

        doubleJump = false;
        bestScore = PlayerPrefs.GetFloat(BestScoreKey, 0f);
    }

    void Update()
    {
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        // O turbo e a pontuacao vivem aqui: um unico lugar que soma, sem depender
        // de quantos objetos de cenario estao na tela.
        turboOn = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Z);
        score += Time.deltaTime * (turboOn ? 25f : 10f);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isOnGround)
            {
                Jump();
                doubleJump = true;
            }
            else if (doubleJump)
            {
                Jump();
                doubleJump = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            playerAnim.SetInteger("DeathType_int", 1);
            playerAnim.SetBool("Death_b", true);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 2);

            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetFloat(BestScoreKey, bestScore);
                PlayerPrefs.Save();
            }
        }
    }

    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isOnGround = false;
        playerAnim.SetTrigger("Jump_trig");
        dirtParticle.Stop();
        playerAudio.PlayOneShot(jumpSound, 1);
    }

    public bool IsTurboOn()
    {
        return turboOn && !gameOver;
    }

    void OnGUI()
    {
        BuildStyles();

        hudStyle.normal.textColor = IsTurboOn() ? NeonLime : NeonCyan;
        GUI.Label(new Rect(24, 18, 460, 40), "PONTOS  " + Mathf.FloorToInt(score), hudStyle);

        hudStyle.normal.textColor = NeonPink;
        GUI.Label(new Rect(24, 54, 460, 40), "RECORDE  " + Mathf.FloorToInt(Mathf.Max(bestScore, score)), hudStyle);

        if (!gameOver)
        {
            hudStyle.normal.textColor = IsTurboOn() ? NeonLime : new Color(1f, 1f, 1f, 0.45f);
            GUI.Label(new Rect(24, 90, 460, 40), "SHIFT = TURBO   ESPACO = PULO DUPLO", hudStyle);
            return;
        }

        bigStyle.normal.textColor = NeonPink;
        GUI.Label(new Rect(0, Screen.height / 2f - 60, Screen.width, 60), "FIM DA FUGA", bigStyle);

        bigStyle.normal.textColor = NeonCyan;
        GUI.Label(new Rect(0, Screen.height / 2f, Screen.width, 40), "R para correr de novo", bigStyle);
    }

    void BuildStyles()
    {
        if (hudStyle != null)
        {
            return;
        }

        hudStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 22,
            fontStyle = FontStyle.Bold
        };

        bigStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 40,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
    }
}
