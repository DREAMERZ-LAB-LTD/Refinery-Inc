using UnityEngine;

namespace IdleArcade.Core
{
    public class ContainerSavedStatus : MonoBehaviour
    {
        [SerializeField] private TransactionContainer container;

        protected virtual int SavedAmount
        {
            set { PlayerPrefs.SetInt("Progress", value); }
            get
            {
                if (PlayerPrefs.HasKey("Progress"))
                    return PlayerPrefs.GetInt("Progress");
                else
                    return 0;
            }
        }


        protected virtual void Start()
        {
            container.Add(SavedAmount);
            container.OnChangedValue += OnUpdate;
        }

        protected virtual void OnDestroy()
        {
            container.OnChangedValue -= OnUpdate;
        }


        private void OnUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
        {
            SavedAmount = currnet;
        }
    }
}