using UnityEngine;

namespace Tutorial
{
    public class TriggerableTutorial : MonoBehaviour
    {
   
        public delegate void OnEvent(int index);
        public OnEvent OnTriggerBegin;
        public OnEvent OnTriggerEnd;

        public virtual void TriggerBegin(int index)
        {
            if (OnTriggerBegin != null)
                OnTriggerBegin.Invoke(index);
        }

        public virtual void TriggerEnd(int index)
        {
            if (OnTriggerEnd != null)
                OnTriggerEnd.Invoke(index);
        }
    }
}