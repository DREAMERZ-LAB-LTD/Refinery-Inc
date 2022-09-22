using UnityEngine;

namespace Tutorial
{
    public class TriggerableTutorial : MonoBehaviour
    {
        public delegate void OnEvent();
        public OnEvent OnTriggerBegin;
        public OnEvent OnTriggerEnd;

        public virtual void TriggerBegin()
        {
            if (OnTriggerBegin != null)
                OnTriggerBegin.Invoke();
        }

        public virtual void TriggerEnd()
        {
            if (OnTriggerEnd != null)
                OnTriggerEnd.Invoke();
        }
    }
}