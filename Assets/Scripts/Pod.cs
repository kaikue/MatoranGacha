using UnityEngine;

public class Pod : MonoBehaviour
{
    private bool clickable = true;

    private void OnMouseDown()
    {
        if (!clickable) return;
        clickable = false;

        transform.parent.gameObject.GetComponent<MatoranGenerator>().OpenPod();
        Destroy(gameObject);
    }
}
