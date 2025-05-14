using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using TMPro;
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

    public GameObject podLeft;
    public GameObject podRight;
    public List<GameObject> masks;
    private GameObject activeMask;
    public GameObject head;
    public List<GameObject> bodyParts;
    public List<GameObject> feet;
    public GameObject matoranParts;
    public string Name;
    public string Village;
    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip clatterSound;
    public AudioClip completeSound;
    public AudioClip clickSound;
    private const float FADE_IN_TIME = 1f;
    public AnimationCurve fadeCurve;
    public GameObject completeUI;
    public TMP_Text nameText;
    public TMP_Text nameText2;
    public TMP_Text villageText;
    public TMP_Text screenshotText;
    public CanvasGroup mouseTipCanvas;
    private Coroutine mouseTipCoroutineLids;
    private Coroutine mouseTipCoroutineParts;

    [HideInInspector]
    public bool headComplete = false;
    [HideInInspector]
    public bool torsoComplete = false;
    [HideInInspector]
    public int limbsComplete = 0;
    [HideInInspector]
    public bool maskComplete = false;

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
        { "re", 1 },
        { "ran", 1 },
        { "ti", 1 },
        { "di", 1 },
        { "pi", 1 },
    };

    private string GenerateName()
    {
        List<string> syllablesList = new List<string>();
        foreach (KeyValuePair<string, int> kv in syllableFrequencies)
        {
            //for (int i = 0; i < kv.Value; i++)
            //{
            syllablesList.Add(kv.Key);
            //}
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

    private byte[] GetHash(string inputString)
    {
        using (HashAlgorithm algorithm = SHA256.Create())
        {
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }

    // https://stackoverflow.com/a/6839784
    private string GetHashString(string inputString)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in GetHash(inputString))
            sb.Append(b.ToString("X2"));

        return sb.ToString();
    }

    private bool IsBannedName()
    {
        List<string> bannedIDs = new List<string>() {
            "A926DA4BE269C2C6BAB65A965584EBB174C6AAEFB63A6859DEFC43D0702D7C96",
            "D30C229855C5A3709FCF2DC6DFBA05C57261BF0A6FEB3944DA34EAE608B2F04E",
            "04484745C9A219C2A18E739754D9A3BBC71F0DC961B3F86A9FD46F09C7948BEB",
            "9E488BBFA97A6DA89346397C4839F13B4EB42E38D89439D36A8B161BE1D0E045",
            "01B46AE0F255E074711ED7AB468C03C10E5E93098844308063A0748EED3552CF",
            "10B3776B12C3ACCD55B19EBC0D70D31B6740B27D57D4177B6DE9EC8C6D2EC4A8"
        };
        return bannedIDs.Contains(GetHashString(name));
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
                rareColors = new List<string>() { "dark green", "brown", "dark blue", "lime", "sand blue", "sand green", "sand purple", "violet" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr medium blue", "tr clear", "tr neon green", "tr purple" };
                break;
            case 1:
                Village = "Ga-Koro";
                mainColors = new List<string>() { "medium blue", "blue", "dark blue", "teal", "violet", "sand blue" };
                rareColors = new List<string>() { "pink", "lime", "white", "orange", "yellow", "light gray", "sand red", "sand purple", "sand green" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr medium blue", "tr clear", "tr neon green", "tr purple" };
                break;
            case 2:
                Village = "Onu-Koro";
                mainColors = new List<string>() { "black", "dark gray", "purple", "dark green" };
                rareColors = new List<string>() { "violet", "dark blue", "tan", "orange", "dark red", "sand red", "sand blue", "sand green", "sand purple", "light gray" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr clear", "tr purple" };
                break;
            case 3:
                Village = "Le-Koro";
                mainColors = new List<string>() { "teal", "lime", "green", "dark green", "sand green" };
                rareColors = new List<string>() { "yellow", "orange", "pink", "red", "white", "dark blue", "sand red", "sand purple", "sand blue", "violet", "purple" };
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr blue", "tr medium blue", "tr clear", "tr neon green", "tr purple" };
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
                kaukauColors = new List<string>() { "tr red", "tr orange", "tr yellow", "tr black", "tr green", "tr clear", "tr purple" };
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

        string footColor;
        r = Random.Range(0, masks.Count);
        activeMask = masks[r];
        activeMask.SetActive(true);
        if (r == 0)
        {
            int r2 = Random.Range(0, kaukauColors.Count);
            foreach (Material mat in activeMask.GetComponent<MeshRenderer>().materials)
            {
                mat.color = colors[kaukauColors[r2]];
            }

            footColor = GetRandomColor(mainColors, rareColors);
        }
        else
        {
            string maskColor = GetRandomColor(mainColors, rareColors);
            foreach (Material mat in activeMask.GetComponent<MeshRenderer>().materials)
            {
                mat.color = colors[maskColor];
            }

            if (Random.Range(0f, 1f) < SAME_HEAD_FOOT_COLOR_CHANCE)
            {
                footColor = maskColor;
            }
            else
            {
                footColor = GetRandomColor(mainColors, rareColors);
            }
        }

        foreach (GameObject foot in feet)
        {
            foot.GetComponent<MeshRenderer>().material.color = colors[footColor];
        }
    }

    private void Start()
    {
        completeUI.SetActive(false);
        screenshotText.alpha = 0;
        audioSource = GetComponent<AudioSource>();
        do
        {
            Name = GenerateName();
        } while (IsBannedName());
        nameText.text = Name;
        nameText2.text = Name;
        GenerateParts();
        villageText.text = Village;
        mouseTipCanvas.alpha = 0;
        mouseTipCoroutineLids = StartCoroutine(MouseTipCoroutineLids());
    }

    private void AddRandomImpulse(GameObject go)
    {
        //go.GetComponent<Rigidbody>().AddExplosionForce(10f, go.transform.position, 10f);
        float r = 4f;
        Vector3 force = new Vector3(Random.Range(-r, r), Random.Range(-r, r), Random.Range(-r, r));
        go.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }

    public void OpenPod()
    {
        head.GetComponent<MatoranPart>().clickable = true;
        AddRandomImpulse(head);
        activeMask.GetComponent<MatoranPart>().clickable = true;
        AddRandomImpulse(activeMask);
        foreach (GameObject bodyPart in bodyParts)
        {
            bodyPart.GetComponent<MatoranPart>().clickable = true;
            AddRandomImpulse(bodyPart);
        }
        foreach (GameObject foot in feet)
        {
            foot.GetComponent<MatoranPart>().clickable = true;
            AddRandomImpulse(foot);
        }

        float openForce = 15f;
        float randomExtra = 4f;
        podLeft.GetComponent<MeshCollider>().enabled = false;
        podLeft.GetComponent<Rigidbody>().isKinematic = false;
        podLeft.GetComponent<Rigidbody>().useGravity = true;
        podLeft.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Vector3 openLeftForce = Vector3.left * openForce + Vector3.up * Random.Range(-randomExtra, randomExtra) + Vector3.forward * Random.Range(-randomExtra, randomExtra);
        podLeft.GetComponent<Rigidbody>().AddForce(openLeftForce, ForceMode.Impulse);
        podRight.GetComponent<MeshCollider>().enabled = false;
        podRight.GetComponent<Rigidbody>().isKinematic = false;
        podRight.GetComponent<Rigidbody>().useGravity = true;
        podRight.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        Vector3 openRightForce = Vector3.right * openForce + Vector3.up * Random.Range(-randomExtra, randomExtra) + Vector3.forward * Random.Range(-randomExtra, randomExtra);
        podRight.GetComponent<Rigidbody>().AddForce(openRightForce, ForceMode.Impulse);

        audioSource.PlayOneShot(openSound);
        audioSource.PlayOneShot(clatterSound);

        StopCoroutine(mouseTipCoroutineLids);
        SetOutlineAlpha(podLeft.GetComponent<Outline>(), 0);
        SetOutlineAlpha(podRight.GetComponent<Outline>(), 0);
        mouseTipCanvas.alpha = 0;
        mouseTipCoroutineParts = StartCoroutine(MouseTipCoroutineParts());
    }

    public void PartComplete(string part)
    {
        StopCoroutine(mouseTipCoroutineParts);
        mouseTipCanvas.alpha = 0;
        SetAllPartsOutlineAlpha(0);

        if (part == "head")
        {
            headComplete = true;
        }
        if (part == "torso")
        {
            torsoComplete = true;
        }
        if (part == "limb")
        {
            limbsComplete++;
        }
        if (part == "mask")
        {
            maskComplete = true;
        }

        if (headComplete && torsoComplete && maskComplete && limbsComplete == 4)
        {
            StartCoroutine(CompleteMatoran());
        }
    }

    private IEnumerator CompleteMatoran()
    {
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(completeSound);
        CanvasGroup canvas = completeUI.GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        completeUI.SetActive(true);
        for (float t = 0; t < FADE_IN_TIME; t += Time.deltaTime)
        {
            float animT = fadeCurve.Evaluate(t / FADE_IN_TIME);
            canvas.alpha = animT;
            yield return new WaitForEndOfFrame();
        }
    }

    public void Screenshot()
    {
        audioSource.PlayOneShot(clickSound);
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] pngData = texture.EncodeToPNG();
        Destroy(texture);

        string encodedText = System.Convert.ToBase64String(pngData);

#if !UNITY_EDITOR
        copyClipboard(encodedText);
#endif

        screenshotText.alpha = 1;
        yield return new WaitForSeconds(3f);
        for (float t = 0; t < FADE_IN_TIME; t += Time.deltaTime)
        {
            screenshotText.alpha = 1 - t / FADE_IN_TIME;
            yield return new WaitForEndOfFrame();
        }
        screenshotText.alpha = 0;
    }

    public void Restart()
    {
        Controller controller = FindAnyObjectByType<Controller>();
        controller.GetComponent<AudioSource>().Play();
        controller.Respawn();
        Destroy(gameObject);
    }

    private void SetOutlineAlpha(Outline o, float a)
    {
        o.OutlineColor = new Color(o.OutlineColor.r, o.OutlineColor.g, o.OutlineColor.b, a);
    }

    private IEnumerator MouseTipCoroutineLids()
    {
        Outline leftOutline = podLeft.GetComponent<Outline>();
        Outline rightOutline = podRight.GetComponent<Outline>();
        while (true)
        {
            yield return new WaitForSeconds(5f);
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                mouseTipCanvas.alpha = t;
                SetOutlineAlpha(leftOutline, t);
                SetOutlineAlpha(rightOutline, t);
                yield return new WaitForEndOfFrame();
            }
            SetOutlineAlpha(leftOutline, 1);
            SetOutlineAlpha(rightOutline, 1);
            mouseTipCanvas.alpha = 1;
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                mouseTipCanvas.alpha = 1 - t;
                SetOutlineAlpha(leftOutline, 1 - t);
                SetOutlineAlpha(rightOutline, 1 - t);
                yield return new WaitForEndOfFrame();
            }
            SetOutlineAlpha(leftOutline, 0);
            SetOutlineAlpha(rightOutline, 0);
            mouseTipCanvas.alpha = 0;
        }
    }

    private void SetAllPartsOutlineAlpha(float a)
    {
        //SetOutlineAlpha(activeMask.GetComponent<Outline>(), a);
        SetOutlineAlpha(head.GetComponent<Outline>(), a);
        foreach (GameObject bodyPart in bodyParts)
        {
            SetOutlineAlpha(bodyPart.GetComponent<Outline>(), a);
        }
        foreach (GameObject foot in feet)
        {
            SetOutlineAlpha(foot.GetComponent<Outline>(), a);
        }
    }

    private IEnumerator MouseTipCoroutineParts()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                mouseTipCanvas.alpha = t;
                SetAllPartsOutlineAlpha(t);
                yield return new WaitForEndOfFrame();
            }
            SetAllPartsOutlineAlpha(1);
            mouseTipCanvas.alpha = 1;
            for (float t = 0; t < 1f; t += Time.deltaTime)
            {
                mouseTipCanvas.alpha = 1 - t;
                SetAllPartsOutlineAlpha(1 - t);
                yield return new WaitForEndOfFrame();
            }
            SetAllPartsOutlineAlpha(0);
            mouseTipCanvas.alpha = 0;
        }
    }

    public IEnumerator HighlightHead()
    {
        Outline headOutline = head.GetComponent<Outline>();
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            SetOutlineAlpha(headOutline, t);
            yield return new WaitForEndOfFrame();
        }
        SetOutlineAlpha(headOutline, 1);
        for (float t = 0; t < 1f; t += Time.deltaTime)
        {
            SetOutlineAlpha(headOutline, 1 - t);
            yield return new WaitForEndOfFrame();
        }
        SetOutlineAlpha(headOutline, 0);
    }

    [DllImport("__Internal")]
    private static extern void copyClipboard(string encodedText);
}
