using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject m_Template;
    [SerializeField] private float m_SpawnTime = 2f;
    [SerializeField] private float m_Distance = 0.2f;

    private float m_ElapsedTime;

    // Update is called once per frame
    void Update()
    {
        m_ElapsedTime += Time.deltaTime;

        if(m_ElapsedTime > m_SpawnTime)
        {
            var random = Random.Range(-m_Distance, m_Distance);

            var newObject = GameObject.Instantiate(m_Template);
            newObject.transform.position = this.transform.position + new Vector3(random, 0, 0);
            m_ElapsedTime = 0f;
        }
    }
}
