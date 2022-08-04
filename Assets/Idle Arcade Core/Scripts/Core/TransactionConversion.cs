using System.Collections;
using UnityEngine;
namespace IdleArcade.Core
{
    public class TransactionConversion : TransactionSource
    {
        [SerializeField, Tooltip("Source container where from the item convert to anoter")] 
        protected TransactionContainer from;
        [SerializeField, Tooltip("Pending conversion request count")] 
        protected int prendingRequestCount = 0;

        protected override void Awake()
        {
            base.Awake();

            from.OnFilled += OnFromContainerFilledUp;
            container.OnChangedValue += OnContainerUpdate;
        }
        private void OnDestroy()
        {
            from.OnFilled -= OnFromContainerFilledUp;
            container.OnChangedValue -= OnContainerUpdate;
        }
            

        protected override void OnEnable()
        {
            StopRoutine();
            routine = StartCoroutine(ConversionRoutine());
        }

        private void OnContainerUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
        {
            if (delta > 0)
                return;

            if(currnet == max - 1)
                OnFromContainerFilledUp();
        }

        /// <summary>
        /// Adding converting request count
        /// </summary>
        private void OnFromContainerFilledUp()
        {
            if (container.willCrossLimit(delta * prendingRequestCount + delta)) return;
            if (!from.isFilledUp) return;

            if (from.TransactFrom(-from.Getamount, from)) 
            {
                prendingRequestCount++;
                OnRequestAdded();
            }
                
        }

        protected virtual void OnRequestAdded() { }
        protected virtual void OnRequestCompleted() { }

        /// <summary>
        /// Converting one item to another
        /// </summary>
        /// <returns></returns>
        private IEnumerator ConversionRoutine()
        {
            while (container)
            {
                while (prendingRequestCount > 0)
                {
                    if (timeIntervalLimit)
                        yield return new WaitForSeconds(timeIntervalLimit.GetCurrent);
                    else
                        yield return null;

                    if (!container.willCrossLimit(delta))
                    {
                        container.Add(delta);
                        prendingRequestCount--;
                        OnRequestCompleted();
                    }
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}