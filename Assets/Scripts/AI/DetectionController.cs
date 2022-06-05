using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

enum DetectionStatus
{
    NOTDETECTED,
    SUSPICIOUS,
    DETECTED
}

public class DetectionController : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public EnemyMovController m_Controller;

    [Header("Visualization")]
    public SpriteRenderer detectionMeter;
    public SpriteRenderer detectedMeter;
    public float lerpSpeed = 20;
    public float showMeterSpeed;
    public Ease showMeterEase;
    public Ease hideMeterEase;
    public float showdetMeterSpeed;
    public float detectedShakeDuration = 0.9f;
    public float detectedShakeIntensity = 1f;
    private float initMeterScale;
    private float initdetMeterScale;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();


    private DetectionStatus m_Status = DetectionStatus.NOTDETECTED;
    public float DetectionDecayValue = -10f;
    public float DetectionIncreaseValue = 20f;
    public float DetectionMaxValue = 100f;
    private float DetectionValue = 0f;


    private float detectedPercent;
    private int shader_ValueParam;


    private void Awake()
    {
        shader_ValueParam = Shader.PropertyToID("_Value");

        initMeterScale = detectionMeter.transform.localScale.x;
        detectionMeter.transform.localScale = Vector3.zero;

        initdetMeterScale = detectedMeter.transform.localScale.x;
        detectedMeter.transform.localScale = Vector3.zero;
    }
    private void Start()
    {

        StartCoroutine("FindTargets", 0.2f);
    }

    private void Update()
    {
        var oldVal = detectionMeter.material.GetFloat(shader_ValueParam);
        var lerpVal = Mathf.Lerp(oldVal, detectedPercent, Time.deltaTime * lerpSpeed);
        detectionMeter.material.SetFloat(shader_ValueParam, lerpVal);
        detectedMeter.material.SetFloat(shader_ValueParam, lerpVal);
    }
    private IEnumerator FindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            UpdateDetectionStatus();
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

    void UpdateDetectionStatus()
    {
        if (m_Status == DetectionStatus.NOTDETECTED || m_Status == DetectionStatus.SUSPICIOUS)
        {
            if (visibleTargets.Count != 0)
            {
                DetectionValue = Mathf.Clamp(DetectionValue + DetectionIncreaseValue, 0f, 100f);
                detectedPercent = DetectionValue / 100;


                if (DetectionValue == 100)
                {
                    m_Status = DetectionStatus.DETECTED;

                    detectionMeter.enabled = false;
                    detectedMeter.enabled = true;
                    detectedMeter.transform.DOScale(initdetMeterScale, showdetMeterSpeed).SetEase(showMeterEase);
                    detectedMeter.transform.DOShakePosition(detectedShakeDuration, detectedShakeIntensity);
                    m_Controller.SetEnemyDetected(true);
                }
            }
            else
            {
                DetectionValue = Mathf.Clamp(DetectionValue + DetectionDecayValue, 0f, 100f);
                detectedPercent = DetectionValue / 100;
            }

            if (m_Status == DetectionStatus.NOTDETECTED)
            {
                //IF Started to get Suspicious
                if (detectedPercent > 0)
                {
                    m_Status = DetectionStatus.SUSPICIOUS;

                    detectionMeter.enabled = true;
                    detectedMeter.enabled = false;
                    m_Controller.SetEnemySuspicious(true);
                    detectionMeter.transform.DOScale(initMeterScale, showMeterSpeed).SetEase(showMeterEase);
                }
            }
            else
            {
                //If lost suspicion
                if (detectedPercent <= 0)
                {
                    m_Status = DetectionStatus.NOTDETECTED;
                    m_Controller.SetEnemySuspicious(false);
                    detectionMeter.transform.DOScale(0, showMeterSpeed).SetEase(hideMeterEase);
                }
            }
        }
        else
        {
            if (visibleTargets.Count == 0)
            {
                DetectionValue = Mathf.Clamp(DetectionValue + DetectionDecayValue, 0f, 100f);
                detectedPercent = DetectionValue / 100;

                if (DetectionValue == 0)
                {
                    m_Status = DetectionStatus.NOTDETECTED;
                    m_Controller.SetEnemyDetected(false);

                    detectedMeter.transform.DOScale(0, showMeterSpeed).SetEase(hideMeterEase);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angle, bool isGlobal)
    {
        if (!isGlobal)
            angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
}
