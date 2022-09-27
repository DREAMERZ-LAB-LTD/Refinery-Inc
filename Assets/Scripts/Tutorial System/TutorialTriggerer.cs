using UnityEngine;
using UnityEngine.Events;
namespace Tutorial
{
    public class TutorialTriggerer : TriggerableTutorial
    {
        [System.Flags]private enum TriggerMode
        {
            None        = 0x000000,
            Awake       = 0x000001,
            OnDestroy   = 0x000010,
            OnEnable    = 0x000100,
            OnDisable   = 0x001000,
            OnMouseDown = 0x010000,
            OnMouseUp   = 0x100000
        }

        [Header("Triggerer Setup")]
        [SerializeField] TriggerMode triggerBeginMode = TriggerMode.Awake;
        [SerializeField] TriggerMode triggerEndMode = TriggerMode.OnMouseDown;
        [SerializeField] TriggerMode triggeredMode = TriggerMode.Awake;
        [SerializeField] private string triggerMask;

        float arrivalTime = 0;

        protected void OnEnable()
        {
            arrivalTime = Time.time;
            if ((triggeredMode & TriggerMode.OnEnable) == TriggerMode.OnEnable)
                OnTriggered();

            if ((triggerBeginMode & TriggerMode.OnEnable) == TriggerMode.OnEnable)
                FireEvent(0);
            if ((triggerEndMode & TriggerMode.OnEnable) == TriggerMode.OnEnable)
                FireEvent(1);
        }

        private  void Awake()
        {
            if ((triggeredMode & TriggerMode.Awake) == TriggerMode.Awake)
                OnTriggered();

            if ((triggerBeginMode & TriggerMode.Awake) == TriggerMode.Awake)
                FireEvent(0);
            if((triggerEndMode & TriggerMode.Awake) == TriggerMode.Awake)
                FireEvent(1);
            
        }
        protected virtual void OnDestroy()
        {
            if ((triggeredMode & TriggerMode.OnDestroy) == TriggerMode.OnDestroy)
                OnTriggered();

            if ((triggerBeginMode & TriggerMode.OnDestroy) == TriggerMode.OnDestroy)
                FireEvent(0);
            if ((triggerEndMode & TriggerMode.OnDestroy) == TriggerMode.OnDestroy)
                FireEvent(1);
        }
        private void OnDisable()
        {
            if ((triggeredMode & TriggerMode.OnDisable) == TriggerMode.OnDisable)
                OnTriggered();

            if ((triggerBeginMode & TriggerMode.OnDisable) == TriggerMode.OnDisable)
                FireEvent(0);
            if ((triggerEndMode & TriggerMode.OnDisable) == TriggerMode.OnDisable)
                FireEvent(1);
        }

        private void Update()
        {
            if (arrivalTime + 1f > Time.time) return;

            if (Input.GetMouseButtonDown(0))
            {
                if ((triggeredMode & TriggerMode.OnMouseDown) == TriggerMode.OnMouseDown)
                    OnTriggered();

                if ((triggerBeginMode & TriggerMode.OnMouseDown) == TriggerMode.OnMouseDown)
                    FireEvent(0);
                if ((triggerEndMode & TriggerMode.OnMouseDown) == TriggerMode.OnMouseDown)
                    FireEvent(1);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if ((triggeredMode & TriggerMode.OnMouseUp) == TriggerMode.OnMouseUp)
                    OnTriggered();

                if ((triggerBeginMode & TriggerMode.OnMouseUp) == TriggerMode.OnMouseUp)
                    FireEvent(0);
                if ((triggerEndMode & TriggerMode.OnMouseUp) == TriggerMode.OnMouseUp)
                    FireEvent(1);
            }
        }
    }
}