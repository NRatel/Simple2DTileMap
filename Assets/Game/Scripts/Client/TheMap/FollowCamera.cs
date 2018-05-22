using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform m_TargetTransform;

    private void Start()
    {
        GetComponent<Camera>().orthographicSize = Config.c_FOV;
    }

    public void SetTarget(Transform targetTransform)
    {
        m_TargetTransform = targetTransform;
        transform.localPosition = new Vector3(m_TargetTransform.localPosition.x, 20, m_TargetTransform.localPosition.z);
    }

    public void MoveToTarget()
    {
        transform.localPosition = new Vector3(m_TargetTransform.localPosition.x, 20, m_TargetTransform.localPosition.z);
    }

    private void LateUpdate()
    {
        if (m_TargetTransform != null)
        {
            MoveToTarget();
        }
    }
}
