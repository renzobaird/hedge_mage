using UnityEngine;

public class BookSceneInitializer : MonoBehaviour
{
    public Transform slotContainer;

    void Start()
    {
        if (WordProgressManager.Instance != null)
        {
            WordProgressManager.Instance.slotContainer = slotContainer;
            WordProgressManager.Instance.SetWord(WordProgressManager.Instance.targetWord);
        }
    }
}