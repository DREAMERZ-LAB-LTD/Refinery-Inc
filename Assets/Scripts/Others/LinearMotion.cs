using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LinearMotion : MonoBehaviour
{
    [Header("Motion Setup")]
    [SerializeField] private float speed = 1;
    [SerializeField] private Transform target;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnMotinBegin;
    [SerializeField] private UnityEvent OnMotinEnd;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(MotionRoutine(pointA.position, pointB.position));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
        

    private IEnumerator MotionRoutine(Vector3 a, Vector3 b)
    { 
        float t = 0;
        OnMotinBegin.Invoke();
        while (t < 1)
        {
            t += speed * Time.deltaTime;
            t = Mathf.Clamp01(t);
            target.position = Vector3.Lerp(a, b, t);
            yield return null;
        }
        target.position = b;
        OnMotinEnd.Invoke();
    }
 
}
