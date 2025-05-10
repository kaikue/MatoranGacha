using System.Collections.Generic;
using System.IO;
using UnityEngine;

class SyllableFrequencyList
{
    public List<SyllableFrequency> items;
}

class SyllableFrequency
{
    public string syllable;
    public int frequency;
}

public class MatoranGenerator : MonoBehaviour
{
    public string Name;

    private const float TWO_SYLLABLES_CHANCE = 0.6f;

    private Dictionary<string, int> syllableFrequencies = new Dictionary<string, int>() {
        { "ta", 8 },
        { "hu", 5 },
        { "ga", 2 },
        { "li", 2 },
        { "o", 3 },
        { "nu", 5 },
        { "a", 12 },
        { "po", 1 },
        { "ha", 4 },
        { "tu", 3 },
        { "le", 1 },
        { "wa", 3 },
        { "ko", 8 },
        { "pa", 3 },
        { "ka", 14 },
        { "ja", 2 },
        { "la", 2 },
        { "ku", 7 },
        { "ma", 16 },
        { "ru", 9 },
        { "ne", 3 },
        { "pu", 3 },
        { "tai", 3 },
        { "fu", 2 },
        { "ki", 8 },
        { "ngu", 1 },
        { "to", 2 },
        { "ro", 1 },
        { "pe", 1 },
        { "ke", 3 },
        { "or", 1 },
        { "kan", 2 },
        { "ri", 2 },
        { "da", 1 },
        { "lu", 2 },
        { "kai", 1 },
        { "ni", 2 },
        { "ya", 1 },
        { "fa", 1 },
        { "mo", 2 },
        { "vi", 2 },
        { "ra", 4 },
        { "dan", 1 },
        { "mu", 1 },
        { "i", 1 },
        { "na", 3 },
        { "ho", 2 },
        { "va", 3 },
        { "no", 1 },
        { "whe", 1 },
        { "tau", 1 },
        { "ju", 1 },
        { "mai", 1 },
        { "wai", 1 },
        { "sa", 1 },
        { "hi", 2 },
        { "si", 1 },
        { "fo", 1 },
        { "kau", 2 },
        { "hau", 1 },
        { "mi", 1 },
        { "rau", 1 },
        { "mau", 1 },
        { "vo", 1 },
        { "lo", 2 },
        { "du", 1 },
        { "ca", 2 },
        { "bo", 1 },
        { "vu", 1 }
    };

    private string GenerateName()
    {
        List<string> syllablesList = new List<string>();
        foreach (KeyValuePair<string, int> kv in syllableFrequencies)
        {
            for (int i = 0; i < kv.Value; i++)
            {
                syllablesList.Add(kv.Key);
            }
        }
        string name = "";
        int syllables = Random.Range(0f, 1f) < TWO_SYLLABLES_CHANCE ? 2 : 3;
        for (int i = 0; i < syllables; i++)
        {
            string syllable;
            do
            {
                int r = Random.Range(0, syllablesList.Count);
                syllable = syllablesList[r];
            } while (name.EndsWith(syllable[0]));
            name += syllable;
        }
        return name;
    }

    private void Start()
    {
        Name = GenerateName();
    }
}
