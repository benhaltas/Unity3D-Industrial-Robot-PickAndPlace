using UnityEngine;

public class Counter : MonoBehaviour
{
    // Unity Members
    [SerializeField] TMPro.TextMeshProUGUI m_TextMeshPro;

    // Members
    private int m_CounterValue;

    // Non public methods
    private void OnTriggerEnter(Collider other)
    {
        m_CounterValue++;
        m_TextMeshPro.text = m_CounterValue.ToString();
    }
}
