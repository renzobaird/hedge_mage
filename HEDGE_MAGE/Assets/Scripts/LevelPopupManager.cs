using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPopupManager : MonoBehaviour
{
    public static LevelPopupManager Instance;

    [Header("Popup References")]
    public GameObject levelFailPopup;
    public GameObject levelCompletePopup;
    public GameObject bookPopup;
    public GameObject buttonBook; // Assign this in the Inspector

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

    public void ShowLevelFailPopup()
    {
        if (levelFailPopup != null)
        {
            levelFailPopup.SetActive(true);
            HideBookButton();
            Time.timeScale = 0f;
        }
    }

    public void ShowLevelCompletePopup()
    {
        if (levelCompletePopup != null)
        {
            levelCompletePopup.SetActive(true);
            HideBookButton();
            Time.timeScale = 0f;
        }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
