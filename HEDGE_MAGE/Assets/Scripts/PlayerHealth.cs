using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxLives = 5;
    public int maxHealth = 100;
    public float respawnDelay = 2f;
    public Transform spawnPoint;
    private float killZoneImmunityTime = 0.8f;
    private float lastDamageTime = -1f;


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
        if (!IsAlive()) return;

        currentHealth -= damageTaken;
        currentHealth = Mathf.Max(currentHealth, 0);
        lastDamageTime = Time.time; // Track when damage was taken

        Debug.Log($"Player took {damageTaken} damage. Current Health: {currentHealth}", this);
        UpdateUI();

        if (currentHealth <= 0)
        {
            LoseLife();
        }
    }


    void LoseLife()
    {
        if (!IsAlive()) return;

        currentLives--;
        Debug.Log($"Player lost a life! Lives left: {currentLives}", this);

        if (currentLives > 0)
        {
            Respawn();
        }
        else
        {
            Die();
        }

        UpdateUI();
    }

    void Die()
    {
        Debug.Log("Player Died!", this);
        Invoke(nameof(RestartScene), respawnDelay);
    }

    void Respawn()
    {
        Debug.Log("Player Respawning!", this);
        transform.position = initialSpawnPoint;
        currentHealth = maxHealth;
        UpdateUI();
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    void SetInitialSpawnPoint()
    {
        initialSpawnPoint = spawnPoint != null ? spawnPoint.position : transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("KillZone"))
        {
            // Prevent accidental kill if player was just hit
            if (Time.time - lastDamageTime < killZoneImmunityTime)
            {
                Debug.Log("KillZone ignored due to recent damage.");
                return;
            }

            Debug.Log("Player entered KillZone.", this);
            currentHealth = 0;
            UpdateUI();
            LoseLife();
        }
    }

}
