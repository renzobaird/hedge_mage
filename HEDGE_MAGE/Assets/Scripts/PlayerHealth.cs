// using UnityEngine;
// using TMPro;

// public class PlayerHealth : MonoBehaviour
// {
//     [Header("Health Settings")]
//     public int maxLives = 3;
//     public int maxHealth = 100;
//     public float respawnDelay = 2f;
//     public float damageCooldown = 1f;
//     public float respawnImmunityTime = 1.5f;
//     public Transform spawnPoint;

//     [Header("UI References")]
//     public TMP_Text livesText;
//     public TMP_Text healthText;

//     [Header("Animation")]
//     public Animator animator;
//     private PlayerMovement playerMovement;

//     private int currentLives;
//     private int currentHealth;
//     private Vector3 initialSpawnPoint;
//     private float lastDamageTime = -10f;
//     private bool isDead = false;
//     public bool IsDead => isDead;

//     void Start()
//     {
//         currentLives = maxLives;
//         currentHealth = maxHealth;
//         playerMovement = GetComponent<PlayerMovement>();
//         initialSpawnPoint = spawnPoint != null ? spawnPoint.position : transform.position;
//         transform.position = initialSpawnPoint;
//         UpdateUI();
//     }

//     public void TakeDamage(int damageTaken)
//     {
//         if (!IsAlive() || isDead || Time.time - lastDamageTime < damageCooldown) return;

//         currentHealth -= damageTaken;
//         currentHealth = Mathf.Max(currentHealth, 0);
//         lastDamageTime = Time.time;
//         UpdateUI();

//         if (currentHealth <= 0)
//         {
//             StartDeathSequence();
//         }
//     }

//     void StartDeathSequence()
//     {
//         if (!IsAlive()) return;

//         isDead = true;
//         if (animator != null)
//         {
//             animator.SetTrigger("Die");
//         }

//         if (playerMovement != null)
//         {
//             playerMovement.SetMovementEnabled(false);
//         }

//         Invoke(nameof(LoseLife), 0.5f);
//     }

//     void LoseLife()
//     {
//         currentLives--;
//         UpdateUI();

//         if (currentLives > 0)
//         {
//             Invoke(nameof(Respawn), respawnDelay);
//         }
//         else
//         {
//             if (LevelPopupManager.Instance != null)
//             {
//                 LevelPopupManager.Instance.ShowLevelFailPopup();
//             }
//         }
//     }

//     void Respawn()
//     {
//         transform.position = initialSpawnPoint;
//         currentHealth = maxHealth;
//         lastDamageTime = Time.time;
//         isDead = false;
//         UpdateUI();

//         if (playerMovement != null)
//         {
//             playerMovement.SetMovementEnabled(true);
//         }
//     }

//     bool IsAlive()
//     {
//         return currentLives > 0;
//     }

//     void UpdateUI()
//     {
//         if (livesText != null)
//         {
//             livesText.text = $"Lives: {currentLives}";
//         }

//         if (healthText != null)
//         {
//             healthText.text = $"Health: {currentHealth}";
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("KillZone"))
//         {
//             if (Time.time - lastDamageTime < respawnImmunityTime || !IsAlive() || isDead) return;

//             currentHealth = 0;
//             UpdateUI();
//             StartDeathSequence();
//         }

//         LetterObject letterObj = other.GetComponent<LetterObject>();
//         if (letterObj != null)
//         {
//             if (WordProgressManager.Instance != null)
//             {
//                 WordProgressManager.Instance.CollectLetter(letterObj.letter);
//             }
//             Destroy(other.gameObject);
//         }
//     }
// }


using UnityEngine;
using TMPro;
using System; // Needed for TimeSpan formatting
using UnityEngine.UI; // For Image component

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxLives = 3;
    public int maxHealth = 100;
    public float respawnDelay = 2f;
    public float damageCooldown = 1f;
    public float respawnImmunityTime = 1.5f;
    public Transform spawnPoint;

    [Header("UI References")]
    public Image lifeImage; // NEW: UI Image to show life sprite
    public Sprite life3Sprite;
    public Sprite life2Sprite;
    public Sprite life1Sprite;
    public TMP_Text healthText;
    public TMP_Text timerText; // Reference to the *in-game* timer UI text

    [Header("Animation")]
    public Animator animator;
    private PlayerMovement playerMovement;

    private int currentLives;
    private int currentHealth;
    private Vector3 initialSpawnPoint;
    private float lastDamageTime = -10f;
    private bool isDead = false;
    public bool IsDead => isDead;

    private bool levelEnded = false;

    void Start()
    {
        currentLives = maxLives;
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        initialSpawnPoint = spawnPoint != null ? spawnPoint.position : transform.position;
        transform.position = initialSpawnPoint;
        UpdateUI();
        levelEnded = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!levelEnded && timerText != null)
        {
            float timeElapsed = Time.timeSinceLevelLoad;
            int minutes = Mathf.FloorToInt(timeElapsed / 60F);
            int seconds = Mathf.FloorToInt(timeElapsed % 60F);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    public void TakeDamage(int damageTaken)
    {
        if (!IsAlive() || isDead || Time.time - lastDamageTime < damageCooldown) return;

        currentHealth -= damageTaken;
        currentHealth = Mathf.Max(currentHealth, 0);
        lastDamageTime = Time.time;
        UpdateUI();

        if (currentHealth <= 0)
        {
            StartDeathSequence();
        }
    }

    void StartDeathSequence()
    {
        if (!IsAlive() || isDead) return;

        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }

        Invoke(nameof(LoseLife), 0.5f);
    }

    void LoseLife()
    {
        currentLives--;
        UpdateUI();

        if (currentLives > 0)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            levelEnded = true;
            if (LevelPopupManager.Instance != null)
            {
                float finalTime = Time.timeSinceLevelLoad;
                LevelPopupManager.Instance.ShowLevelFailPopup(finalTime);
            }
            else
            {
                Debug.LogWarning("LevelPopupManager instance not found. Cannot show fail popup.");
            }
        }
    }

    void Respawn()
    {
        transform.position = initialSpawnPoint;
        currentHealth = maxHealth;
        lastDamageTime = Time.time - (damageCooldown - respawnImmunityTime);
        isDead = false;
        UpdateUI();

        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(true);
        }
    }

    bool IsAlive()
    {
        return currentLives > 0;
    }

    void UpdateUI()
    {
        if (lifeImage != null)
        {
            switch (currentLives)
            {
                case 3:
                    lifeImage.sprite = life3Sprite;
                    lifeImage.enabled = true;
                    break;
                case 2:
                    lifeImage.sprite = life2Sprite;
                    lifeImage.enabled = true;
                    break;
                case 1:
                    lifeImage.sprite = life1Sprite;
                    lifeImage.enabled = true;
                    break;
                default:
                    lifeImage.enabled = false;
                    break;
            }
        }

        if (healthText != null)
        {
            healthText.text = $"{currentHealth} HP";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            if (Time.time - lastDamageTime < respawnImmunityTime || !IsAlive() || isDead) return;

            currentHealth = 0;
            UpdateUI();
            StartDeathSequence();
        }

        LetterObject letterObj = other.GetComponent<LetterObject>();
        if (letterObj != null)
        {
            if (WordProgressManager.Instance != null)
            {
                WordProgressManager.Instance.CollectLetter(letterObj.letter);
            }
            else
            {
                Debug.LogWarning("WordProgressManager instance not found. Cannot collect letter.");
            }
            Destroy(other.gameObject);
        }

        if (other.CompareTag("LevelEndTrigger"))
        {
            CompleteLevel();
        }
    }

    public void CompleteLevel()
    {
        Debug.Log("Level Complete Triggered!");
        if (levelEnded) return;

        levelEnded = true;
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }

        if (LevelPopupManager.Instance != null)
        {
            float finalTime = Time.timeSinceLevelLoad;
            LevelPopupManager.Instance.ShowLevelCompletePopup(finalTime);
        }
        else
        {
            Debug.LogWarning("LevelPopupManager instance not found. Cannot show complete popup.");
        }
    }
}
