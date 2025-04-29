using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WordProgressManager : MonoBehaviour
{
    public static WordProgressManager Instance;

    public GameObject letterSlotPrefab;
    public Transform slotContainer;

    [SerializeField] private string[] wordList = { "APPLE", "HOUSE", "LIGHT", "BRICK", "WATER" };
    private string targetWord;
    private int currentWordIndex = 0;
    private HashSet<int> collectedIndexes = new HashSet<int>();
    private List<LetterSlotUI> letterSlots = new List<LetterSlotUI>();


    public GameObject bookPopup;
    public GameObject levelCompletePopup;
    public GameObject levelFailPopup;

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

    private void Start()
    {
        StartNewGame();
    }

    public void StartNewGame()
    {
        targetWord = GetNextWord().ToUpper();
        collectedIndexes.Clear();
        SetupUISlots();
    }

    private string GetNextWord()
    {
        if (wordList.Length == 0) return "PLACE";
        string word = wordList[currentWordIndex % wordList.Length];
        currentWordIndex++;
        return word;
    }

    private void SetupUISlots()
    {
        Debug.Log($"Clearing previous letter slots. Existing count: {slotContainer.childCount}");

        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }
        letterSlots.Clear();

        Debug.Log($"Setting up slots for new word: {targetWord}");

        foreach (char c in targetWord)
        {
            GameObject slotObj = Instantiate(letterSlotPrefab, slotContainer);
            LetterSlotUI slot = slotObj.GetComponent<LetterSlotUI>();


            if (slot == null)
            {
                Debug.LogError("Instantiated LetterSlot prefab is missing the LetterSlot component.");
                continue;
            }

            Sprite uncollectedSprite = LetterSpriteDatabase.Instance.GetUncollectedSprite(c);
            if (uncollectedSprite == null)
            {
                Debug.LogWarning($"Missing uncollected sprite for letter: {c}");
            }

            slot.SetLetter(c, uncollectedSprite);
            letterSlots.Add(slot);

            Debug.Log($"Slot created for letter: {c}");
        }

        Debug.Log($"Total slots created: {letterSlots.Count}");
    }

    public void CollectLetter(char collected)
    {
        collected = char.ToUpper(collected);

        for (int i = 0; i < targetWord.Length; i++)
        {
            if (targetWord[i] == collected && !collectedIndexes.Contains(i))
            {
                collectedIndexes.Add(i);
                break;
            }
        }

        UpdateSlots();

        if (IsWordComplete())
        {
            ShowLevelCompletePopup();
        }
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (i < letterSlots.Count)
            {
                char letter = targetWord[i];
                Sprite sprite = collectedIndexes.Contains(i)
                    ? LetterSpriteDatabase.Instance.GetCollectedSprite(letter)
                    : LetterSpriteDatabase.Instance.GetUncollectedSprite(letter);

                letterSlots[i].SetCollectedSprite(sprite);
            }
        }
    }

    public bool IsLetterCollected(char c)
    {
        c = char.ToUpper(c);
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (targetWord[i] == c && collectedIndexes.Contains(i))
                return true;
        }
        return false;
    }

    public string GetTargetWord()
    {
        return targetWord;
    }

    private bool IsWordComplete()
    {
        return collectedIndexes.Count == targetWord.Length;
    }

    private void ShowLevelCompletePopup()
    {
        if (levelCompletePopup != null)
        {
            levelCompletePopup.SetActive(true);
            Time.timeScale = 0f;
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

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}