using UnityEngine;

public class Gripper : MonoBehaviour
{
    [SerializeField] private FixedJoint m_FixedJoint;

    private Rigidbody m_DetectedObject;

    public bool HasObject => m_FixedJoint != null && m_FixedJoint.connectedBody != null;
    public bool CanGrab => m_DetectedObject != null && !HasObject;

    public bool TryGrab()
    {
        if (!CanGrab)
        {
            return false;
        }

        m_FixedJoint.connectedBody = m_DetectedObject;
        return true;
    }

    public void Release()
    {
        if (m_FixedJoint != null)
        {
            m_FixedJoint.connectedBody = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            m_DetectedObject = other.attachedRigidbody;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!HasObject && other.attachedRigidbody == m_DetectedObject)
        {
            m_DetectedObject = null;
        }
    }
}
