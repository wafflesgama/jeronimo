using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum DetectionStatus
{
    NOTDETECTED,
    DETECTED
}

public class DetectionController : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    private EnemyMovController m_Controller;

    private DetectionStatus m_Status = DetectionStatus.NOTDETECTED;
    public float DetectionDecayValue = -10f;
    public float DetectionIncreaseValue = 20f;
    public float DetectionMaxValue = 100f;
    private float DetectionValue = 0f;
    

    private void Start()
    {
        m_Controller = GetComponent<EnemyMovController>();
        StartCoroutine("FindTargets",0.2f);   
    }

    private IEnumerator FindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            updateDetectionStatus();
        }
    }


    private void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    void updateDetectionStatus()
    {
        Debug.Log(DetectionValue);
        if(m_Status == DetectionStatus.NOTDETECTED)
        {
            if (visibleTargets.Count != 0)
            {
                DetectionValue = Mathf.Clamp(DetectionValue + DetectionIncreaseValue, 0f, 100f);
                
                if(DetectionValue == 100)
                {
                    m_Status = DetectionStatus.DETECTED;
                    m_Controller.setEnemyDetected(true);
                }
            }
            else
            {
                DetectionValue = Mathf.Clamp(DetectionValue + DetectionDecayValue, 0f, 100f);
            }
        }
        else
        {
            if (visibleTargets.Count == 0)
            {
                DetectionValue = Mathf.Clamp(DetectionValue + DetectionDecayValue, 0f, 100f);

                if (DetectionValue == 0)
                {
                    m_Status = DetectionStatus.NOTDETECTED;
                    m_Controller.setEnemyDetected(false);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angle, bool isGlobal)
    {
        if(!isGlobal)
            angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
