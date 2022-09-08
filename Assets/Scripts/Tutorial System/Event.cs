using UnityEngine.Events;

namespace Tutorial
{ 
    [System.Serializable]
    public class Event
    {
        public UnityEvent OnTriggerBegin;
        public UnityEvent OnTriggerEnd;
        public UnityEvent OnTriggered;
    }
}
