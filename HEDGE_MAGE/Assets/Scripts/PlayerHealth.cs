using UnityEngine;
using UnityEngine.SceneManagement; // Needed for scene management
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxLives = 5;
    public int maxHealth = 100;
    public float respawnDelay = 2f; // Delay before respawn or loading failure scene
    public Transform spawnPoint;
    private float killZoneImmunityTime = 0.8f;
    private float lastDamageTime = -1f;

    [Header("Scene Management")]
    public string failureSceneName = "05_Level Failure"; // Name of the scene to load on game over

    [Header("UI References")]
    public TMP_Text livesText;
    public TMP_Text healthText;

    private int currentLives;
    private int currentHealth;
    private Vector3 initialSpawnPoint;

    void Start()
    {
        currentLives = maxLives;
        currentHealth = maxHealth;
        SetInitialSpawnPoint();
        UpdateUI();
    }

    public void TakeDamage(int damageTaken)
    {
        // Don't take damage if already dead (waiting for respawn/scene load)
        // or if lives are already 0 (waiting for scene load)
        if (currentHealth <= 0 || !IsAlive()) return;

        currentHealth -= damageTaken;
        currentHealth = Mathf.Max(currentHealth, 0); // Clamp health to minimum 0
        lastDamageTime = Time.time; // Track when damage was taken

        Debug.Log($"Player took {damageTaken} damage. Current Health: {currentHealth}", this);
        UpdateUI();

        // Check if health dropped to 0 or below
        if (currentHealth <= 0)
        {
            LoseLife();
        }
    }

    void LoseLife()
    {
        // This function is called when health reaches 0
        if (!IsAlive()) return; // Should not happen if called from TakeDamage, but good safety check

        currentLives--;
        Debug.Log($"Player lost a life! Lives left: {currentLives}", this);
        UpdateUI(); // Update UI immediately after losing a life

        if (currentLives > 0)
        {
            // Player has more lives, respawn after a delay
            Debug.Log("Player Respawning soon!", this);
            // Use Invoke to delay the Respawn method call
            Invoke(nameof(Respawn), respawnDelay);
        }
        else
        {
            // Player has no lives left, trigger the game over sequence
            Die();
        }
    }

    void Die()
    {
        // This function is called when currentLives reaches 0
        Debug.Log("Player Died! Loading Failure Scene...", this);
        // Use Invoke to delay loading the failure scene
        Invoke(nameof(LoadFailureScene), respawnDelay);
    }

    void Respawn()
    {
        // This function resets the player's state for another try
        Debug.Log("Player Respawning Now!", this);
        transform.position = initialSpawnPoint; // Move player to spawn point
        currentHealth = maxHealth; // Restore health fully
        // Optional: Add brief invincibility frames here if needed
        UpdateUI(); // Update UI with restored health
    }

    // Renamed from RestartScene to be more specific
    void LoadFailureScene()
    {
        Debug.Log($"Loading Scene: {failureSceneName}");
        SceneManager.LoadScene(failureSceneName); // Load the specified failure scene
    }

    bool IsAlive()
    {
        // Checks if the player has lives remaining
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

    void SetInitialSpawnPoint()
    {
        // Sets the initial spawn point, using the object's start position as a fallback
        initialSpawnPoint = spawnPoint != null ? spawnPoint.position : transform.position;
        // Ensure the player starts at the spawn point visually as well
        transform.position = initialSpawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            // Ignore if player just took damage (prevents double hits)
            if (Time.time - lastDamageTime < killZoneImmunityTime)
            {
                Debug.Log("KillZone ignored due to recent damage.");
                return;
            }

            // Don't trigger if already dead or waiting for respawn/scene load
            if (currentHealth <= 0 || !IsAlive()) return;

            Debug.Log("Player entered KillZone.", this);
            currentHealth = 0; // Instantly set health to 0
            UpdateUI(); // Update UI to show 0 health
            LoseLife(); // Trigger the LoseLife logic (which will check lives)
        }
    }
}