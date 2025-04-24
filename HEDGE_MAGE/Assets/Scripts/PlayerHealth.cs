using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxLives = 3;
    public int maxHealth = 100;
    public float respawnDelay = 2f;
    public float damageCooldown = 1f;
    public float respawnImmunityTime = 1.5f;
    public Transform spawnPoint;

    [Header("Scene Management")]
    public string failureSceneName = "05_Level Failure";

    [Header("UI References")]
    public TMP_Text livesText;
    public TMP_Text healthText;

    [Header("Animation")]
    public Animator animator; // Assign in Inspector
    private PlayerMovement playerMovement;

    private int currentLives;
    private int currentHealth;
    private Vector3 initialSpawnPoint;
    private float lastDamageTime = -10f;
    private bool isDead = false;
    public bool IsDead => isDead;


    void Start()
    {
        currentLives = maxLives;
        currentHealth = maxHealth;
        playerMovement = GetComponent<PlayerMovement>();
        initialSpawnPoint = spawnPoint != null ? spawnPoint.position : transform.position;
        transform.position = initialSpawnPoint;
        UpdateUI();
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
        if (!IsAlive()) return;

        isDead = true;
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(false);
        }

        Invoke(nameof(LoseLife), 0.5f); // Slight delay to allow animation to show
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
            Invoke(nameof(LoadFailureScene), respawnDelay);
        }
    }

    void Respawn()
    {
        transform.position = initialSpawnPoint;
        currentHealth = maxHealth;
        lastDamageTime = Time.time;
        isDead = false;
        UpdateUI();

        if (playerMovement != null)
        {
            playerMovement.SetMovementEnabled(true);
        }
    }

    void LoadFailureScene()
    {
        SceneManager.LoadScene(failureSceneName);
    }

    bool IsAlive()
    {
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
            if (Time.time - lastDamageTime < respawnImmunityTime || !IsAlive() || isDead) return;

            currentHealth = 0;
            UpdateUI();
            StartDeathSequence();
        }

        // Letter collection logic
        LetterObject letterObj = other.GetComponent<LetterObject>();
        if (letterObj != null)
        {
            // Notify WordProgressManager of the collection
            if (WordProgressManager.Instance != null)
            {
                WordProgressManager.Instance.CollectLetter(letterObj.letter);
            }

            // Remove the letter from the scene
            Destroy(other.gameObject);
        }
    }
}