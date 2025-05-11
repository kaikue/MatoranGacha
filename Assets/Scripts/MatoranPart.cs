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

    private void Awake()
    {
        goalPos = transform.position;
        goalRotation = transform.rotation;
        transform.position = Vector3.zero;
    }

    private void OnMouseDown()
    {
        if (!clickable) return;

        clickable = false;
        clickedPos = transform.position;
        clickedRotation = transform.rotation;
        gameObject.GetComponent<Collider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        StartCoroutine(MoveToPosition());
    }

    private IEnumerator MoveToPosition()
    {
        for (float t = 0; t < MOVE_TIME; t += Time.fixedDeltaTime)
        {
            float animT = movementCurve.Evaluate(t);
            transform.SetPositionAndRotation(Vector3.Lerp(clickedPos, goalPos, animT), Quaternion.Lerp(clickedRotation, goalRotation, animT));
            yield return new WaitForFixedUpdate();
        }
        transform.SetPositionAndRotation(goalPos, goalRotation);
        FindAnyObjectByType<MatoranGenerator>().PartComplete(partName);
    }
}
