using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject.Destroy(other.gameObject);
    }
}
