using UnityEngine;
using UnityEngine.SceneManagement; // Required for restarting the scene

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 5;        // Maximum number of lives the player has
    public int currentLives;     // Current number of lives the player has
    public float respawnDelay = 2f; // Delay before respawning the player

    public Transform spawnPoint; //add this line
    private Vector3 initialSpawnPoint;

    // Optional:  For displaying UI elements (like lives)
    // public Text livesText; //  You'd need to create a Text element in your scene

    void Start()
    {
        // Initialize current lives
        currentLives = maxLives;
        // Optional: Update UI at start
        // UpdateLivesUI();
        if (spawnPoint != null)
        {
            initialSpawnPoint = spawnPoint.position;
        }
        else
        {
            initialSpawnPoint = transform.position; //if no spawn point is set, use the player's starting position
        }

    }

    // Method to handle taking damage
    public void TakeDamage(int damageAmount) // Make sure to pass in the damage amount
    {
        currentLives -= damageAmount; // Subtract the damage from current lives

        // Optional: Update UI after taking damage
        // UpdateLivesUI();

        if (currentLives <= 0)
        {
            Die(); // Call Die() when lives reach zero
        }
        else
        {
            Respawn(); //call respawn
        }
    }

    // Method for when the player dies (no more lives)
    void Die()
    {
        // Optional: Play death animation/sound
        Debug.Log("Player Died!"); // For debugging

        // Disable player controls or game object
        // gameObject.SetActive(false); // Simple way to "disable" the player

        // Restart the scene after a delay
        Invoke("RestartScene", respawnDelay); // Restart the entire scene
        //OR
        //Respawn(); //just respawn

    }

    void Respawn()
    {
        // Optional: Play respawn animation/sound
        Debug.Log("Player Respawning!");

        //reset player position
        transform.position = initialSpawnPoint;
        //reset lives
        currentLives = maxLives;

        // Enable player controls
        // gameObject.SetActive(true);
    }

    // Method to restart the entire scene
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Optional: Method to update a UI Text element
    // void UpdateLivesUI()
    // {
    //     if (livesText != null)
    //     {
    //         livesText.text = "Lives: " + currentLives.ToString();
    //     }
    // }

    //Collision detection
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with an enemy
        if (other.gameObject.CompareTag("Enemy")) // Make sure your enemy has the tag "Enemy"
        {
            TakeDamage(1); //remove 1 life
            //Destroy(other.gameObject);  //destroy the enemy.  you might not want to do this.
        }
        if (other.gameObject.CompareTag("KillZone"))
        {
            currentLives = 0;
            Die();
        }
    }
}
