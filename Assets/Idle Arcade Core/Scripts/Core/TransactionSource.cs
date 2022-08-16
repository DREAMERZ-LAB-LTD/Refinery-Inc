using System.Collections;
using UnityEngine;

namespace IdleArcade.Core
{
    [RequireComponent(typeof(TransactionContainer))]

    public class TransactionSource : MonoBehaviour
    {
        [SerializeField] private string containerID;
        protected TransactionContainer container;
        public TransactionContainer GetContainer => container;

        [SerializeField] protected Limiter timeIntervalLimit;
        protected const int delta = 1;

        protected Coroutine routine;

        protected virtual void Awake()
        {
            var containers = GetComponents<TransactionContainer>();
            for (int i = 0; i < containers.Length; i++)
                if (containerID == containers[i].GetID)
                {
                    container = containers[i];
                    break;
                }
        }

        private IEnumerator GeneratorRoutine()
        {
            while (container)
            {
                if(timeIntervalLimit)
                    yield return new WaitForSeconds(timeIntervalLimit.GetCurrent);
                else
                    yield return null;

                if (!container.isFilledUp)
                    container.Add(delta);
            }
        }

        /// <summary>
        /// stop the current generator routine
        /// </summary>
        protected void StopRoutine()
        {
            if (routine != null)
                StopCoroutine(routine);
        }

        protected virtual void OnEnable()
        {
            StopRoutine();
            routine = StartCoroutine(GeneratorRoutine());
        }
        protected virtual void OnDisable()
        {
            StopRoutine();
        }
    }
}