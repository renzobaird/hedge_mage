using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPopupManager : MonoBehaviour
{
    public static LevelPopupManager Instance;

    [Header("Popup References")]
    public GameObject levelFailPopup;
    public GameObject levelCompletePopup;
    public GameObject bookPopup;

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
            Time.timeScale = 0f;
        }
    }

    public void ShowLevelCompletePopup()
    {
        if (levelCompletePopup != null)
        {
            levelCompletePopup.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ShowBookPopup()
    {
        if (bookPopup != null)
        {
            bookPopup.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void CloseBookPopup()
    {
        if (bookPopup != null)
        {
            bookPopup.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartNextLevel()
    {
        Time.timeScale = 1f;
        WordProgressManager.Instance.StartNewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
