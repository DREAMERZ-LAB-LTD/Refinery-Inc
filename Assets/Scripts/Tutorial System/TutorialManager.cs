using UnityEngine;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private string id;
        [SerializeField] private TriggerableTutorial[] triggerers;
        [SerializeField] private Event[] events;

        private void Awake()
        {
            for (int i = 0; i < triggerers.Length; i++)
            { 
                triggerers[i].OnTriggerBegin += TriggerBegin;
                triggerers[i].OnTriggerEnd += TriggerEnd;
            }

            int index = CurrentIndex;
            if (index == 0) return;
            for (int i = 0; i <= index; i++)
                events[i].OnTriggered.Invoke();
        }
        private void OnDestroy()
        {
            for (int i = 0; i < triggerers.Length; i++)
            { 
                triggerers[i].OnTriggerBegin -= TriggerBegin;
                triggerers[i].OnTriggerEnd -= TriggerEnd;
            }
        }

        public int CurrentIndex
        {
            get
            {
                if (PlayerPrefs.HasKey(id))
                    return PlayerPrefs.GetInt(id);
                return 0;
            }
            set
            {
                PlayerPrefs.SetInt(id, value);
            }
        }

        public void TriggerBegin(int index)
        {
            events[index].OnTriggerBegin.Invoke();
        }

        public void TriggerEnd(int index)
        {
            events[index].OnTriggerEnd.Invoke();
            CurrentIndex = index;
        }
    }
}