using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject podPrefab;

    private void Start()
    {
        Respawn();
    }

    public void Respawn()
    {
        Instantiate(podPrefab);
    }
}
