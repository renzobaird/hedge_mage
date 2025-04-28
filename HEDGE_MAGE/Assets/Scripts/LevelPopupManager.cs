using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPopupManager : MonoBehaviour
{
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartNextLevel()
    {
        WordProgressManager.Instance.StartNewGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}