using System.Collections.Generic;
using UnityEngine;

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

    private bool isRetrying = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        StartNewGame();
    }

    public void RetrySameWord()
    {
        isRetrying = true;
        StartNewGame();
    }

    public void AdvanceToNextWord()
    {
        isRetrying = false;
        StartNewGame();
    }

    public void StartNewGame()
    {
        targetWord = GetNextWord().ToUpper();
        collectedIndexes.Clear();
        SetupUISlots();

        LetterManager.Instance.ResetLettersForNewWord(targetWord);  // THIS IS NEWLY INSERTED CODE 5/2

    }

    private string GetNextWord()
    {
        if (wordList.Length == 0) return "PLACE";
        return isRetrying ? wordList[(currentWordIndex - 1 + wordList.Length) % wordList.Length] : wordList[currentWordIndex++ % wordList.Length];
    }

    private void SetupUISlots()
    {
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }
        letterSlots.Clear();

        foreach (char c in targetWord)
        {
            GameObject slotObj = Instantiate(letterSlotPrefab, slotContainer);
            LetterSlotUI slot = slotObj.GetComponent<LetterSlotUI>();
            slot.SetLetter(c, LetterSpriteDatabase.Instance.GetUncollectedSprite(c));
            letterSlots.Add(slot);
        }
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

        if (collectedIndexes.Count == targetWord.Length)
        {
            float finalTime = PlayerHealth.Instance.GetElapsedLevelTime();
            LevelPopupManager.Instance.ShowLevelCompletePopup(finalTime);

        }
    }

    private void UpdateSlots()
    {
        for (int i = 0; i < targetWord.Length; i++)
        {
            char letter = targetWord[i];
            Sprite sprite = collectedIndexes.Contains(i)
                ? LetterSpriteDatabase.Instance.GetCollectedSprite(letter)
                : LetterSpriteDatabase.Instance.GetUncollectedSprite(letter);

            letterSlots[i].SetCollectedSprite(sprite);
        }
    }

    public bool IsLetterCollected(char c)
    {
        c = char.ToUpper(c);
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (targetWord[i] == c && collectedIndexes.Contains(i)) return true;
        }
        return false;
    }

    public string GetTargetWord() => targetWord;
}