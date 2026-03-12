using UnityEngine;

public class ManualRobotControl : MonoBehaviour
{
    [SerializeField] private GameObject m_XAxis;
    [SerializeField] private GameObject m_YAxis;
    [SerializeField] private float m_Speed = 1.0f;
    [SerializeField] private Vector2 m_XRange;
    [SerializeField] private Vector2 m_YRange;

    // Update is called once per frame
    private void Update()
    {
        // Frameunabhängige Geschwindigkeit ermitteln
        float speed = Time.deltaTime * m_Speed;

        // Delta X berechnen
        float deltaX = 0f;
        if(Input.GetKey(KeyCode.LeftArrow))
        {
            deltaX += speed;
        }
        if(Input.GetKey(KeyCode.RightArrow))
        {
            deltaX -= speed;
        }

        deltaX = Mathf.Clamp(m_XAxis.transform.localPosition.x + deltaX, m_XRange.x, m_XRange.y);
        m_XAxis.transform.localPosition = new Vector3(deltaX, m_XAxis.transform.localPosition.y, m_XAxis.transform.localPosition.z);


        // Delta Y berechnen
        float deltaY = 0f;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            deltaY += speed;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaY -= speed;
        }
        deltaY = Mathf.Clamp(m_YAxis.transform.localPosition.y + deltaY, m_YRange.x, m_YRange.y);
        m_YAxis.transform.localPosition = new Vector3(m_YAxis.transform.localPosition.x, deltaY, m_YAxis.transform.localPosition.z);
    }
}
