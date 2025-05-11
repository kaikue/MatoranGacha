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
    private AudioSource audioSource;
    private MatoranGenerator controller;

    private void Awake()
    {
        goalPos = transform.position;
        goalRotation = transform.rotation;
        transform.position = Vector3.zero;
        audioSource = GetComponent<AudioSource>();
        controller = FindAnyObjectByType<MatoranGenerator>();
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
        if (partName == "mask" && !FindAnyObjectByType<MatoranGenerator>().headComplete)
        {
            audioSource.PlayOneShot(errorSound);
            return;
        }

        clickable = false;
        clickedPos = transform.position;
        clickedRotation = transform.rotation;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(MoveToPosition());

        if (partName == "head")
        {
            if (controller.torsoComplete) StartCoroutine(PlayConnectSound());
        }
        if (partName == "torso")
        {
            if (controller.headComplete || controller.limbsComplete > 0) StartCoroutine(PlayConnectSound());
        }
        if (partName == "limb")
        {
            if (controller.torsoComplete) StartCoroutine(PlayConnectSound());
        }
        if (partName == "mask")
        {
            StartCoroutine(PlayMaskConnectSound());
        }
    }

    private IEnumerator MoveToPosition()
    {
        controller.PartComplete(partName);
        for (float t = 0; t < MOVE_TIME; t += Time.fixedDeltaTime)
        {
            float animT = movementCurve.Evaluate(t);
            transform.SetPositionAndRotation(Vector3.Lerp(clickedPos, goalPos, animT), Quaternion.Lerp(clickedRotation, goalRotation, animT));
            yield return new WaitForFixedUpdate();
        }
        transform.SetPositionAndRotation(goalPos, goalRotation);
    }
}
