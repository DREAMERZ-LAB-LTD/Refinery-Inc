using UnityEngine;
using UnityEngine.Events;
namespace Tutorial
{
    public class TutorialTriggerer : TriggerableTutorial
    {
        private enum TriggerMode
        {
            None,
            Awake,
            OnDestroy,
            OnMouseDown,
            OnMouseUp,
            OnTriggerEnter,
            OnTriggerExit
        }

        [Header("Triggerer Setup")]
        [SerializeField] TriggerMode triggerBeginMode = TriggerMode.Awake;
        [SerializeField] TriggerMode triggerEndMode = TriggerMode.OnMouseDown;
        [SerializeField] private string triggerMask;


        protected override void Awake()
        {
            base.Awake();

            if(triggerBeginMode == TriggerMode.Awake)
                FireEvent(0);
            else if (triggerEndMode == TriggerMode.Awake)
                FireEvent(1);
            
        }
        protected virtual void OnDestroy()
        {
            if (triggerBeginMode == TriggerMode.OnDestroy)
                FireEvent(0);
            else if (triggerEndMode == TriggerMode.OnDestroy)
                FireEvent(1);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (triggerBeginMode == TriggerMode.OnTriggerEnter)
                FireEvent(0);
            else if (triggerEndMode == TriggerMode.OnTriggerEnter)
                FireEvent(1);
        }
        private void OnTriggerExit(Collider other)
        {
            if (triggerBeginMode == TriggerMode.OnTriggerExit)
                FireEvent(0);
            else if (triggerEndMode == TriggerMode.OnTriggerExit)
                FireEvent(1);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (triggerBeginMode == TriggerMode.OnMouseDown)
                    FireEvent(0);
                else if (triggerEndMode == TriggerMode.OnMouseDown)
                    FireEvent(1);
            }
            else if (Input.GetMouseButtonUp(0))
            { 
                if (triggerBeginMode == TriggerMode.OnMouseUp)
                    FireEvent(0);
                else if(triggerEndMode == TriggerMode.OnMouseUp)
                    FireEvent(1);
            }
        }
    }
}