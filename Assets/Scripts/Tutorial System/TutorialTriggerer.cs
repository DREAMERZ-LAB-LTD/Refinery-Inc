using System.Collections;
using UnityEngine;
using Tutorial;

public class TutorialTriggerer : TriggerableTutorial
{
    private enum TriggerMode
    {
        OnEnable,
        OnDisable,
        OnMouseDown,
        OnMouseUp,
        OnTriggerEnter,
        OnTriggerExit
    }


    [SerializeField] private string triggerMask;
    [SerializeField] TriggerMode triggerBeginMode = TriggerMode.OnEnable;
    [SerializeField] TriggerMode triggerEndMode = TriggerMode.OnDisable;

    protected virtual void OnEnable()
    {
        StartCoroutine(FrameSkipRoutine());
        IEnumerator FrameSkipRoutine()
        {
            yield return new WaitForEndOfFrame();
            if (triggerBeginMode == TriggerMode.OnEnable)
                TriggerBegin();
            if (triggerEndMode == TriggerMode.OnEnable)
                TriggerEnd();
        }
    }
    protected virtual void OnDisable()
    {
        if (triggerBeginMode == TriggerMode.OnDisable)
            TriggerBegin();
        if (triggerEndMode == TriggerMode.OnDisable)
            TriggerEnd();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != triggerMask) return;

        if (triggerBeginMode == TriggerMode.OnTriggerEnter)
            TriggerBegin();
        if (triggerEndMode == TriggerMode.OnTriggerEnter)
            TriggerEnd();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != triggerMask) return;

        if (triggerBeginMode == TriggerMode.OnTriggerExit)
            TriggerBegin();
        if (triggerEndMode == TriggerMode.OnTriggerExit)
            TriggerEnd();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (triggerBeginMode == TriggerMode.OnMouseDown)
                TriggerBegin();
            if (triggerEndMode == TriggerMode.OnMouseDown)
                TriggerEnd();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (triggerBeginMode == TriggerMode.OnMouseUp)
                TriggerBegin();
            if (triggerEndMode == TriggerMode.OnMouseUp)
                TriggerEnd();
        }
    }
}
