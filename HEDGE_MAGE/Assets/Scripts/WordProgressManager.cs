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
    private List<LetterSlot> letterSlots = new List<LetterSlot>();

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
        foreach (Transform child in slotContainer)
        {
            Destroy(child.gameObject);
        }
        letterSlots.Clear();

        foreach (char c in targetWord)
        {
            GameObject slotObj = Instantiate(letterSlotPrefab, slotContainer);
            LetterSlot slot = slotObj.GetComponent<LetterSlot>();
            Sprite uncollectedSprite = LetterSpriteDatabase.Instance.GetUncollectedSprite(c);
            slot.SetLetter(c, uncollectedSprite);
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

        if (IsWordComplete())
        {
            LevelPopupManager.Instance.ShowLevelCompletePopup();
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

    private bool IsWordComplete()
    {
        return collectedIndexes.Count == targetWord.Length;
    }
}