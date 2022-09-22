using UnityEngine;
using UnityEngine.Events;

namespace Tutorial
{
    public class TutorialSequenceManager : MonoBehaviour
    {
        [System.Serializable] public class EventAction
        {
            [Header("Triggerer Setup")]
            [SerializeField] private string id;
            [SerializeField] private TriggerableTutorial triggerer;

            [Header("Callback Events")]
            [SerializeField] private UnityEvent OnTriggerBegin;
            [SerializeField] private UnityEvent OnTriggerEnd;
            [SerializeField] private UnityEvent OnTriggered;

            private void SaveStatus() => PlayerPrefs.SetInt(id, 1);
            public void Init()
            {
                if (PlayerPrefs.GetInt(id) == 0)
                {
                    triggerer.OnTriggerBegin += OnTriggerBegin.Invoke;
                    triggerer.OnTriggerEnd += OnTriggerEnd.Invoke;
                    triggerer.OnTriggerEnd += SaveStatus;
                }
                else
                    OnTriggered.Invoke();

            }
            public void Dispose()
            {
                triggerer.OnTriggerBegin -= OnTriggerBegin.Invoke;
                triggerer.OnTriggerEnd -= OnTriggerEnd.Invoke;
                triggerer.OnTriggerEnd -= SaveStatus;
            }
        }

        [SerializeField] private EventAction[] eventActions;
   
        private void Awake()
        {
            for (int i = 0; i < eventActions.Length; i++)
                eventActions[i].Init();
        }
        private void OnDestroy()
        {
            for (int i = 0; i < eventActions.Length; i++)
                eventActions[i].Dispose();
        }
    }
}