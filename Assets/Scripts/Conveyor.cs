using UnityEngine;

public class Conveyor : MonoBehaviour
{
    // Unity Members
    [SerializeField]
    private float m_Speed = 2.0f;
    [SerializeField]
    private Vector3 m_Direction = Vector3.back;

    // Members
    private Rigidbody m_Rigidbody;

    // Non public methods
    void Awake()
    {
        m_Rigidbody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        m_Rigidbody.position -= m_Direction * m_Speed * Time.deltaTime;
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Direction * m_Speed * Time.deltaTime);
    }
}
