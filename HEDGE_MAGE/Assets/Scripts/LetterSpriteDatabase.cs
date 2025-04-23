using System.Collections.Generic;
using UnityEngine;

public class LetterSpriteDatabase : MonoBehaviour
{
    public static LetterSpriteDatabase Instance;

    [System.Serializable]
    public struct LetterSpritePair
    {
        public char letter;
        public Sprite sprite;
    }

    public List<LetterSpritePair> letterSprites;

    private Dictionary<char, Sprite> letterSpriteDict;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        letterSpriteDict = new Dictionary<char, Sprite>();
        foreach (var pair in letterSprites)
        {
            char upper = char.ToUpper(pair.letter);
            if (!letterSpriteDict.ContainsKey(upper))
            {
                letterSpriteDict.Add(upper, pair.sprite);
            }
        }
    }

    public Sprite GetSpriteForLetter(char c)
    {
        c = char.ToUpper(c);
        if (letterSpriteDict != null && letterSpriteDict.TryGetValue(c, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }
}