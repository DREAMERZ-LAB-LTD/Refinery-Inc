using System.Collections;
using UnityEngine;

namespace Tutorial
{
    public class TutorialTriggerer : TriggerableTutorial
    {
        private enum TriggerMode
        {
            None,
            OnEnable,
            OnDisable,
            OnAwake,
            OnStart,
        }


        [SerializeField] protected int index;

        [SerializeField] TriggerMode triggerBeginMode = TriggerMode.OnEnable;
        [SerializeField] TriggerMode triggerEndMode = TriggerMode.OnDisable;

     
        protected virtual void OnEnable()
        {
            StartCoroutine(FrameSkipRoutine());
            IEnumerator FrameSkipRoutine()
            {
                yield return new WaitForEndOfFrame();
                if (triggerBeginMode == TriggerMode.OnEnable)
                    TriggerBegin(index);
                if (triggerEndMode == TriggerMode.OnEnable)
                    TriggerEnd(index);
            }
        }
        protected virtual void OnDisable()
        {
            if (triggerBeginMode == TriggerMode.OnDisable)
                TriggerBegin(index);
            if (triggerEndMode == TriggerMode.OnDisable)
                TriggerEnd(index);
        }
        protected virtual void Awake()
        {
            if (triggerBeginMode == TriggerMode.OnAwake)
                TriggerBegin(index);
            if (triggerEndMode == TriggerMode.OnAwake)
                TriggerEnd(index);
        }
        protected virtual void Start()
        {
            if (triggerBeginMode == TriggerMode.OnStart)
                TriggerBegin(index);
            if (triggerEndMode == TriggerMode.OnStart)
                TriggerEnd(index);
        }
    }
}