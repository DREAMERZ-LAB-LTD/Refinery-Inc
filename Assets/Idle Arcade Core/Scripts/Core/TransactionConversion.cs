using System.Collections;
using UnityEngine;
namespace IdleArcade.Core
{
    public class TransactionConversion : TransactionSource
    {
        [SerializeField, Tooltip("Source container where from the item conver to anoter")] 
        protected TransactionContainer from;
        [SerializeField, Tooltip("Pending conversion request count")] 
        protected int prendingRequestCount = 0;

        protected override void Awake()
        {
            base.Awake();

            from.OnFilled += OnFromContainerFilledUp;
        }
        private void OnDestroy()
        {
            from.OnFilled -= OnFromContainerFilledUp;
        }
            

        protected override void OnEnable()
        {
            StopRoutine();
            routine = StartCoroutine(ConversionRoutine());
        }

        /// <summary>
        /// Adding converting request count
        /// </summary>
        private void OnFromContainerFilledUp()
        {
            if(from.Add(-from.Getamount))
                prendingRequestCount++;
        }

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

                    container.Add(delta);
                    prendingRequestCount--;
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}