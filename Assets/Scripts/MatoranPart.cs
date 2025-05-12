using System.Collections;
using UnityEngine;

public class MatoranPart : MonoBehaviour
{
    private const float MOVE_TIME = 1f;

    public string partName;
    public AnimationCurve movementCurve;
    [HideInInspector]
    public bool clickable = false;
    private Vector3 goalPos;
    private Quaternion goalRotation;
    private Vector3 clickedPos;
    private Quaternion clickedRotation;
    public AudioClip[] connectSounds;
    public AudioClip[] maskConnectSounds;
    public AudioClip errorSound;
    public AudioClip clickSound;
    private AudioSource audioSource;
    private MatoranGenerator generator;

    private void Awake()
    {
        goalPos = transform.position;
        goalRotation = transform.rotation;
        transform.position = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        generator = GetComponentInParent<MatoranGenerator>();
    }

    private IEnumerator PlayConnectSound()
    {
        yield return new WaitForSeconds(0.8f);
        int r = Random.Range(0, connectSounds.Length);
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(connectSounds[r]);
    }

    private IEnumerator PlayMaskConnectSound()
    {
        yield return new WaitForSeconds(0.9f);
        int r = Random.Range(0, maskConnectSounds.Length);
        audioSource.PlayOneShot(maskConnectSounds[r]);
    }

    private void OnMouseDown()
    {
        if (!clickable) return;
        if (partName == "mask" && !generator.headComplete)
        {
            StartCoroutine(generator.HighlightHead());
            audioSource.PlayOneShot(errorSound);
            return;
        }

        audioSource.PlayOneShot(clickSound);
        clickable = false;
        clickedPos = transform.position;
        clickedRotation = transform.rotation;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        generator.PartComplete(partName);
        StartCoroutine(MoveToPosition());

        if (partName == "head")
        {
            if (generator.torsoComplete) StartCoroutine(PlayConnectSound());
        }
        if (partName == "torso")
        {
            if (generator.headComplete || generator.limbsComplete > 0) StartCoroutine(PlayConnectSound());
        }
        if (partName == "limb")
        {
            if (generator.torsoComplete) StartCoroutine(PlayConnectSound());
        }
        if (partName == "mask")
        {
            StartCoroutine(PlayMaskConnectSound());
        }
    }

    private IEnumerator MoveToPosition()
    {
        for (float t = 0; t < MOVE_TIME; t += Time.fixedDeltaTime)
        {
            float animT = movementCurve.Evaluate(t / MOVE_TIME);
            transform.SetPositionAndRotation(Vector3.Lerp(clickedPos, goalPos, animT), Quaternion.Lerp(clickedRotation, goalRotation, animT));
            yield return new WaitForFixedUpdate();
        }
        transform.SetPositionAndRotation(goalPos, goalRotation);
    }
}
