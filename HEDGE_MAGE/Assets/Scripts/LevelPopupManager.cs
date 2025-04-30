using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelPopupManager : MonoBehaviour
{
    public static LevelPopupManager Instance;

    [Header("Popup References")]
    public GameObject levelFailPopup;
    public GameObject levelCompletePopup;
    public GameObject bookPopup;
    public GameObject buttonBook;

    [Header("Time Display Texts")]
    public TMP_Text failTimeText;      // 👈 assign the text in LevelFailPopup
    public TMP_Text completeTimeText;  // 👈 assign the text in LevelCompletePopup

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

    public void ShowLevelFailPopup(float finalTime)
    {
        if (levelFailPopup != null)
        {
            FormatAndStoreTime(finalTime);
            if (failTimeText != null)
                failTimeText.text = finalTimeFormatted;

            levelFailPopup.SetActive(true);
            HideBookButton();
            Time.timeScale = 0f;
        }
    }

    public void ShowLevelCompletePopup(float finalTime)
    {
        if (levelCompletePopup != null)
        {
            FormatAndStoreTime(finalTime);
            if (completeTimeText != null)
                completeTimeText.text = finalTimeFormatted;

            levelCompletePopup.SetActive(true);
            HideBookButton();
            Time.timeScale = 0f;
        }
    }

    private void FormatAndStoreTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60F);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60F);
        finalTimeFormatted = $"{minutes:00}:{seconds:00}";
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
