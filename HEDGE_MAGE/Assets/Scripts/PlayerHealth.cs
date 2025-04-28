using UnityEngine;
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

    [Header("UI References")]
    public TMP_Text livesText;
    public TMP_Text healthText;

    [Header("Animation")]
    public Animator animator;
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
            if (LevelPopupManager.Instance != null)
            {
                LevelPopupManager.Instance.ShowLevelFailPopup();
            }
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

        LetterObject letterObj = other.GetComponent<LetterObject>();
        if (letterObj != null)
        {
            if (WordProgressManager.Instance != null)
            {
                WordProgressManager.Instance.CollectLetter(letterObj.letter);
            }
            Destroy(other.gameObject);
        }
    }
}