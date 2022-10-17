using UnityEngine;
using UnityEngine.Events;

namespace Tutorial
{
    public class TriggerableTutorial : MonoBehaviour
    {
        [Header("Persistent Setup")]
        [SerializeField, Tooltip("Persistent ID")]
        private string id;

        [Header("Callback Events")]
        [SerializeField] private UnityEvent m_OnTriggerBegin;
        [SerializeField] private UnityEvent m_OnTriggerEnd;
        [SerializeField] private UnityEvent m_OnTriggered;

        protected bool isTriggred
        {
            get => PlayerPrefs.GetInt(id + name) == 1;
            set
            { 
                PlayerPrefs.SetInt(id + name, value ? 1 : 0);
#if UNITY_EDITOR
                m_IsTriggred = value;
#endif

            }
        }

#if UNITY_EDITOR
        [Header("Debug Setup")]
        [SerializeField] private bool m_IsTriggred = false;
        private void OnDrawGizmosSelected()
        {
            isTriggred = m_IsTriggred ;
        }
#endif
        /// <summary>
        /// Fire the event to execute the tutorial content
        /// </summary>
        /// <param name="state">'Trigger Begin = 0', 'Trigger End = 1', 'Triggered = 2' </param>
        public void FireEvent(int state)
        {
            if (isTriggred) return;
            
            switch (state)
            {
                case 0:
                    m_OnTriggerBegin.Invoke();
                    break;
                case 1:
                    isTriggred = true;
                    m_OnTriggerEnd.Invoke();
                    break;
            }
        }

        protected void OnTriggered()
        { 
            if (isTriggred)
                m_OnTriggered.Invoke();
        }
    }
}