using UnityEngine;
using System.Collections;

namespace IdleArcade.Core
{
    public class ContainerSavedStatus : Entity
    {
        [SerializeField] private TransactionContainer container;

        protected virtual int SavedAmount
        {
            set => PlayerPrefs.SetInt(GetID, value);
            get => PlayerPrefs.GetInt(GetID);
        }


        protected virtual void Start()
        {
            StartCoroutine(SkipFrame());
            IEnumerator SkipFrame()
            {
                yield return null;
                container.Add(SavedAmount);
                container.OnChangedValue += OnUpdate;
            }
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