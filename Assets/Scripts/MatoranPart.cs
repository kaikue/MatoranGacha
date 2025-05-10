using UnityEngine;

public class MatoranPart : MonoBehaviour
{
    private Vector3 goalPos;

    private void Awake()
    {
        goalPos = transform.position;
        //transform.position = Vector3.zero;
    }
}
