using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
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
    public SerializedDictionary<string, Color> colors;

    public List<GameObject> masks;
    public GameObject head;
    public List<GameObject> bodyParts;
    public List<GameObject> feet;
    public GameObject matoranParts;
    public string Name;
    public string Village;

    private const float TWO_SYLLABLES_CHANCE = 0.6f;
    private const float RARE_COLOR_CHANCE = 0.3f;
    private const float SAME_HEAD_FOOT_COLOR_CHANCE = 0.5f;

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
        { "vu", 1 },
        { "u", 1 },
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
        name = name[0].ToString().ToUpper() + name.Substring(1);
        return name;
    }

    private string GetRandomColor(List<string> mainColors, List<string> rareColors)
    {
        if (Random.Range(0f, 1f) < RARE_COLOR_CHANCE)
        {
            int r = Random.Range(0, rareColors.Count);
            return rareColors[r];
        }
        else
        {
            int r = Random.Range(0, mainColors.Count);
            return mainColors[r];
        }
    }

    private void GenerateParts()
    {
        if (Random.Range(0f, 1f) < 0.5f)
        {
            head.GetComponent<MeshRenderer>().material.color = colors["light gray"];
        }
        else
        {
            head.GetComponent<MeshRenderer>().material.color = colors["dark gray"];
        }

        List<string> mainColors;
        List<string> rareColors;
        List<string> kaukauColors;
        int r = Random.Range(0, 6);
        switch (r)
        {
            case 0:
                Village = "Ta-Koro";
                mainColors = new List<string>() { "red", "orange", "yellow", "black", "dark red", "dark gray" };
                rareColors = new List<string>() { "dark green", "brown", "dark blue", "purple", "lime", "sand blue", "sand green", "sand purple", "violet" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr medium blue", "tr clear" };
                break;
            case 1:
                Village = "Ga-Koro";
                mainColors = new List<string>() { "medium blue", "blue", "dark blue", "teal", "violet", "sand blue" };
                rareColors = new List<string>() { "pink", "lime", "white", "orange", "yellow", "light gray", "sand red", "sand purple", "sand green" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr medium blue", "tr clear" };
                break;
            case 2:
                Village = "Onu-Koro";
                mainColors = new List<string>() { "black", "dark gray", "purple", "dark green" };
                rareColors = new List<string>() { "violet", "dark blue", "tan", "orange", "dark red", "sand red", "sand blue", "sand green", "sand purple", "light gray" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr clear" };
                break;
            case 3:
                Village = "Le-Koro";
                mainColors = new List<string>() { "teal", "lime", "green", "dark green", "sand green" };
                rareColors = new List<string>() { "yellow", "orange", "pink", "red", "white", "dark blue", "sand red", "sand purple", "sand blue", "violet", "purple" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr medium blue", "tr clear" };
                break;
            case 4:
                Village = "Ko-Koro";
                mainColors = new List<string>() { "white", "medium blue", "light gray", "dark gray", "sand blue" };
                rareColors = new List<string>() { "black", "dark blue", "sand red", "sand green", "sand purple", "pink" };
                kaukauColors = new List<string>() { "tr red", "tr yellow", "tr black", "tr green", "tr blue", "tr medium blue", "tr clear" };
                break;
            case 5:
                Village = "Po-Koro";
                mainColors = new List<string>() { "brown", "tan", "black", "dark orange" };
                rareColors = new List<string>() { "orange", "dark red", "light gray", "dark gray", "dark green", "sand red", "sand blue", "sand green", "sand purple", "pink", "violet" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr clear" };
                break;
            default:
                Village = "Mata Nui";
                mainColors = new List<string>() { "dark gray" };
                rareColors = new List<string>() { "dark gray" };
                kaukauColors = new List<string>() { "tr black" };
                break;
        }

        r = Random.Range(0, mainColors.Count);
        foreach (GameObject bodyPart in bodyParts)
        {
            bodyPart.GetComponent<MeshRenderer>().material.color = colors[mainColors[r]];
        }
        print("body: " + mainColors[r]);

        string footColor;
        r = Random.Range(0, masks.Count);
        masks[r].SetActive(true);
        if (r == 0)
        {
            int r2 = Random.Range(0, kaukauColors.Count);
            foreach (Material mat in masks[r].GetComponent<MeshRenderer>().materials)
            {
                mat.color = colors[kaukauColors[r2]];
            }
            print("kaukau: " + kaukauColors[r2]);

            footColor = GetRandomColor(mainColors, rareColors);
        }
        else
        {
            string maskColor = GetRandomColor(mainColors, rareColors);
            foreach (Material mat in masks[r].GetComponent<MeshRenderer>().materials)
            {
                mat.color = colors[maskColor];
            }
            print("mask: " + maskColor);

            if (Random.Range(0f, 1f) < SAME_HEAD_FOOT_COLOR_CHANCE)
            {
                footColor = maskColor;
            }
            else
            {
                footColor = GetRandomColor(mainColors, rareColors);
            }
        }

        print("foot: " + footColor);
        foreach (GameObject foot in feet)
        {
            foot.GetComponent<MeshRenderer>().material.color = colors[footColor];
        }
    }

    private void Start()
    {
        Name = GenerateName();
        GenerateParts();
    }
}
