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
    public TMP_Text livesText;
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

    // Flag to prevent the timer from updating after the level ends
    private bool levelEnded = false;

    void Start()
    {
        currentLives = maxLives;
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        initialSpawnPoint = spawnPoint != null ? spawnPoint.position : transform.position;
        transform.position = initialSpawnPoint;
        UpdateUI(); // Initial UI update for health/lives
        levelEnded = false; // Ensure level hasn't ended at start
        Time.timeScale = 1f; // Ensure time scale is normal at start
    }

    void Update()
    {
        // --- Timer Logic ---
        // Only update the timer if the level hasn't ended
        if (!levelEnded && timerText != null)
        {
            // Get the time elapsed since the scene loaded
            float timeElapsed = Time.timeSinceLevelLoad; // Use Time.timeSinceLevelLoad for per-level time

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
        if (!IsAlive() || isDead) return; // Added isDead check here too

        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }

        Invoke(nameof(LoseLife), 0.5f); // Delay slightly for animation
    }

    void LoseLife()
    {
        currentLives--;
        UpdateUI();

        if (currentLives > 0)
        {
            Invoke(nameof(Respawn), respawnDelay);
        }
        else // Player has run out of lives
        {
            levelEnded = true; // Stop the UI timer update
            if (LevelPopupManager.Instance != null)
            {
                // <<< MODIFIED: Get current time and pass it to the popup manager
                float finalTime = Time.timeSinceLevelLoad; // Get time *before* pausing
                LevelPopupManager.Instance.ShowLevelFailPopup(finalTime);
            }
            else
            {
                Debug.LogWarning("LevelPopupManager instance not found. Cannot show fail popup.");
                // Optionally still pause here if needed
                // Time.timeScale = 0f;
            }
            // Game Over logic - Don't respawn
        }
    }

    void Respawn()
    {
        transform.position = initialSpawnPoint;
        currentHealth = maxHealth;
        // Set lastDamageTime slightly in the past relative to respawn time to allow immunity
        lastDamageTime = Time.time - (damageCooldown - respawnImmunityTime);
        isDead = false;
        UpdateUI();

        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(true);
        }
        // Reset any relevant animation states if needed
        // if (animator != null) animator.ResetTrigger("Die"); // Example
    }

    bool IsAlive()
    {
        // Consider isDead flag as well, though currentLives <= 0 is the main check for game over
        return currentLives > 0;
    }

    void UpdateUI()
    {
        if (livesText != null)
        {
            livesText.text = $"Lives: {currentLives}";
        }

        if (healthText != null)
        {
            healthText.text = $"Health: {currentHealth}";
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
           // Use the respawnImmunityTime directly for the check after respawn
            if (Time.time - lastDamageTime < respawnImmunityTime || !IsAlive() || isDead) return;

            currentHealth = 0;
            UpdateUI();
            StartDeathSequence(); // Trigger the death sequence
        }
        // Handle LetterObject collection... (kept your existing logic)
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

        // <<< ADDED: Example for Level Completion Trigger
        if (other.CompareTag("LevelEndTrigger")) // Use a tag like "LevelEndTrigger" for your goal object
        {
             CompleteLevel();
        }
    }

    // <<< ADDED: Method to handle level completion
    public void CompleteLevel()
    {
        if (levelEnded) return; // Prevent multiple completions

        levelEnded = true; // Stop the UI timer update
        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false); // Stop player movement
        }

        if (LevelPopupManager.Instance != null)
        {
            // Get current time and pass it to the popup manager
            float finalTime = Time.timeSinceLevelLoad; // Get time *before* pausing
            LevelPopupManager.Instance.ShowLevelCompletePopup(finalTime);
        }
        else
        {
            Debug.LogWarning("LevelPopupManager instance not found. Cannot show complete popup.");
            // Optionally still pause here if needed
            // Time.timeScale = 0f;
        }
        // You might want to play a victory sound or animation here
    }
}