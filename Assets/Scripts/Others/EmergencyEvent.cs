using System.Collections;
using UnityEngine;
using IdleArcade.Core;
using UnityEngine.Events;

public class EmergencyEvent : MonoBehaviour
{
    [SerializeField] private Vector2 delayRange = Vector2.up;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnEventBegin;
    [SerializeField] private UnityEvent OnEventEnd;



    public void OnBegin()
    {
        var delay = Random.Range(delayRange.x, delayRange.y);
        StopAllCoroutines();
        StartCoroutine(EventRoutine(delay));
    }


    public void OnEnd()
    {
        StopAllCoroutines();
        OnEventEnd.Invoke();
    }


    private IEnumerator EventRoutine(float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        OnEventBegin.Invoke();
        Debug.Log("Event Begin");
    }

}
