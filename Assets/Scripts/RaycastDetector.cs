using UnityEngine;

public class RaycastDetector : MonoBehaviour
{
    [SerializeField] private float m_Length;

    private float m_HitDistance = -1f;

    public float HitDistance => m_HitDistance;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.forward * m_Length, Color.red);
        if(Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, m_Length))
        {
            m_HitDistance = hit.distance;
        }
        else
        {
            m_HitDistance = -1f;
        }
    }
}
