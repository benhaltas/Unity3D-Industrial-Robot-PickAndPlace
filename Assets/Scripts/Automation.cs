using UnityEngine;

public class Automation : MonoBehaviour
{
    [SerializeField] private RaycastDetector m_RaycastDetector;
    [SerializeField] private GameObject m_XAxis;
    [SerializeField] private GameObject m_YAxis;
    [SerializeField] private Gripper m_Gripper;

    [SerializeField] private float m_XSpeed = 1.2f;
    [SerializeField] private float m_YSpeed = 1.2f;
    [SerializeField] private Vector2 m_XRange = new Vector2(-0.9f, 0.9f);
    [SerializeField] private Vector2 m_YRange = new Vector2(0f, 0.5f);

    [SerializeField] private float m_CarryY = 0.5f;
    [SerializeField] private float m_PickY = 0.18f;
    [SerializeField] private float m_DropY = 0.18f;
    [SerializeField] private float m_DropX = 0.59f;
    [SerializeField] private float m_WaitX = -0.6f;
    [SerializeField] private float m_GrabRetrySeconds = 0.35f;
    [SerializeField] private float m_XTolerance = 0.02f;
    [SerializeField] private float m_YTolerance = 0.02f;

    private enum State
    {
        Track,
        LowerPick,
        Grab,
        RaiseAfterPick,
        MoveDrop,
        LowerDrop,
        Release,
        RaiseHome
    }

    private State m_State = State.Track;

    private float m_TargetX;
    private float m_GrabRetryTimer;
    private bool m_HasPendingPick;
    private float m_PendingPickX;

    private void FixedUpdate()
    {
        if (m_RaycastDetector == null || m_XAxis == null || m_YAxis == null || m_Gripper == null)
        {
            return;
        }

        UpdatePendingPickFromSensor();

        switch (m_State)
        {
            case State.Track:
                bool hasLiveBox = m_RaycastDetector.HitDistance > 0f;
                if (m_HasPendingPick)
                {
                    m_TargetX = m_PendingPickX;
                }
                else if (hasLiveBox)
                {
                    m_TargetX = Mathf.Clamp(-1f + m_RaycastDetector.HitDistance + 0.15f, m_XRange.x, m_XRange.y);
                }
                else
                {
                    m_TargetX = Mathf.Clamp(m_WaitX, m_XRange.x, m_XRange.y);
                }

                MoveX(m_TargetX);

                if (m_HasPendingPick && Mathf.Abs(m_XAxis.transform.localPosition.x - m_TargetX) <= m_XTolerance)
                {
                    m_State = State.LowerPick;
                }
                break;

            case State.LowerPick:
                if (MoveY(m_PickY))
                {
                    m_GrabRetryTimer = 0f;
                    m_State = State.Grab;
                }
                break;

            case State.Grab:
                if (m_Gripper.TryGrab())
                {
                    m_HasPendingPick = false;
                    m_State = State.RaiseAfterPick;
                }
                else
                {
                    m_GrabRetryTimer += Time.fixedDeltaTime;
                    if (m_GrabRetryTimer >= m_GrabRetrySeconds)
                    {
                        m_HasPendingPick = false;
                        m_State = State.Track;
                    }
                }
                break;

            case State.RaiseAfterPick:
                if (MoveY(m_CarryY))
                {
                    m_State = State.MoveDrop;
                }
                break;

            case State.MoveDrop:
                if (MoveX(m_DropX))
                {
                    m_State = State.LowerDrop;
                }
                break;

            case State.LowerDrop:
                if (MoveY(m_DropY))
                {
                    m_State = State.Release;
                }
                break;

            case State.Release:
                m_Gripper.Release();
                m_State = State.RaiseHome;
                break;

            case State.RaiseHome:
                if (MoveY(m_CarryY))
                {
                    MoveX(Mathf.Clamp(m_WaitX, m_XRange.x, m_XRange.y));
                    if (Mathf.Abs(m_XAxis.transform.localPosition.x - Mathf.Clamp(m_WaitX, m_XRange.x, m_XRange.y)) <= m_XTolerance)
                    {
                        m_State = State.Track;
                    }
                }
                break;
        }
    }

    private bool MoveX(float targetX)
    {
        Vector3 p = m_XAxis.transform.localPosition;
        float nx = Mathf.MoveTowards(p.x, targetX, m_XSpeed * Time.fixedDeltaTime);
        nx = Mathf.Clamp(nx, m_XRange.x, m_XRange.y);
        m_XAxis.transform.localPosition = new Vector3(nx, p.y, p.z);
        return Mathf.Abs(nx - targetX) <= m_XTolerance;
    }

    private bool MoveY(float targetY)
    {
        Vector3 p = m_YAxis.transform.localPosition;
        float ny = Mathf.MoveTowards(p.y, targetY, m_YSpeed * Time.fixedDeltaTime);
        ny = Mathf.Clamp(ny, m_YRange.x, m_YRange.y);
        m_YAxis.transform.localPosition = new Vector3(p.x, ny, p.z);
        return Mathf.Abs(ny - targetY) <= m_YTolerance;
    }

    private void UpdatePendingPickFromSensor()
    {
        if (m_RaycastDetector.HitDistance <= 0f)
        {
            return;
        }

        m_PendingPickX = Mathf.Clamp(-1f + m_RaycastDetector.HitDistance + 0.15f, m_XRange.x, m_XRange.y);
        m_HasPendingPick = true;
    }
}
