using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // <<< ADDED: Namespace for TextMeshPro
using System; // <<< ADDED: Namespace for TimeSpan formatting

public class LevelPopupManager : MonoBehaviour
{
    public static LevelPopupManager Instance;

    [Header("Popup References")]
    public GameObject levelFailPopup;
    public GameObject levelCompletePopup;
    public GameObject bookPopup;
    public GameObject buttonBook; // Assign this in the Inspector

    // <<< ADDED: References to the Text components on the popups
    [Header("UI Text References")]
    public TMP_Text failTimeText;       // Assign the TextMeshPro component from levelFailPopup
    public TMP_Text completeTimeText;   // Assign the TextMeshPro component from levelCompletePopup

    // Store the final time formatted string
    private string finalTimeFormatted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // <<< MODIFIED: Accept final time as a parameter
    public void ShowLevelFailPopup(float finalTime)
    {
        if (levelFailPopup != null)
        {
            FormatAndStoreTime(finalTime); // Format the time

            // <<< ADDED: Update the time text on the fail popup
            if (failTimeText != null)
            {
                failTimeText.text = $"{finalTimeFormatted}";
            }
            else
            {
                Debug.LogWarning("Fail Time Text reference not set in LevelPopupManager.");
            }

            levelFailPopup.SetActive(true);
            HideBookButton();
            Time.timeScale = 0f; // Pause the game *after* getting the time
        }
    }

    // <<< MODIFIED: Accept final time as a parameter
    public void ShowLevelCompletePopup(float finalTime)
    {
        if (levelCompletePopup != null)
        {
            FormatAndStoreTime(finalTime); // Format the time

            // <<< ADDED: Update the time text on the complete popup
            if (completeTimeText != null)
            {
                completeTimeText.text = $"{finalTimeFormatted}";
            }
            else
            {
                Debug.LogWarning("Complete Time Text reference not set in LevelPopupManager.");
            }

            levelCompletePopup.SetActive(true);
            HideBookButton();
            Time.timeScale = 0f; // Pause the game *after* getting the time
        }
    }

    // Helper method to format the time consistently
    private void FormatAndStoreTime(float timeInSeconds)
    {
        // Using the same formatting as your PlayerHealth script
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        finalTimeFormatted = $"{minutes:00}:{seconds:00}";

        // Alternative using TimeSpan:
        // TimeSpan timeSpan = TimeSpan.FromSeconds(timeInSeconds);
        // finalTimeFormatted = $"{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }


    public void ShowBookPopup()
    {
        if (bookPopup != null)
        {
            bookPopup.SetActive(true);
            HideBookButton();
            Time.timeScale = 0f;
        }
    }

    public void CloseBookPopup()
    {
        if (bookPopup != null)
        {
            bookPopup.SetActive(false);
            ShowBookButton();
            Time.timeScale = 1f;
        }
    }

    public void RestartLevel()
    {
        Debug.Log("Restart button clicked. Restarting level.");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartNextLevel()
    {
        Debug.Log("Next level button clicked.");
        Time.timeScale = 1f;
        // Make sure this loads the *actual* next level, not restarts the current one
        // Example: SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // You'll need a more robust scene management system for a full game.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Simple next level logic
    }

    private void HideBookButton()
    {
        if (buttonBook != null)
        {
            buttonBook.SetActive(false);
        }
    }

    private void ShowBookButton()
    {
        if (buttonBook != null)
        {
            buttonBook.SetActive(true);
        }
    }
}

// using UnityEngine;

// public class LevelPopupManager : MonoBehaviour
// {
//     public GameObject BookPopup;
//     public GameObject LevelFailPopup;
//     public GameObject LevelCompletePopup;

//     public static LevelPopupManager Instance { get; private set; }

//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(gameObject);
//             return;
//         }

//         Instance = this;
//     }


//     public void ShowBookPopup()
//     {
//         BookPopup.SetActive(true);
//     }

//     public void HideBookPopup()
//     {
//         BookPopup.SetActive(false);
//     }

//     public void ShowLevelFailPopup()
//     {
//         LevelFailPopup.SetActive(true);
//     }

//     public void ShowLevelCompletePopup()
//     {
//         LevelCompletePopup.SetActive(true);
//     }

//     public void RestartLevel()
//     {
//         LevelFailPopup.SetActive(false);

//         // Reset game word/letters
//         WordProgressManager.Instance.RestartLevel();

//         // Reset player health and position
//         PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
//         if (playerHealth != null)
//         {
//             playerHealth.FullReset();  // Make sure this resets health/lives and repositions the player
//         }

//         // Reactivate gameplay (in case it was frozen)
//         Time.timeScale = 1f;
//     }

//     public void StartNextLevel()
//     {
//         LevelCompletePopup.SetActive(false);

//         // Start new word
//         WordProgressManager.Instance.StartNewGame();

//         // Reset player health and position
//         PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
//         if (playerHealth != null)
//         {
//             playerHealth.FullReset();
//         }

//         Time.timeScale = 1f;
//     }
// }
