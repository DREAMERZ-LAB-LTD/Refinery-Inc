using System.Collections;
using UnityEngine;

namespace IdleArcade.Core
{
    public class TransactionBridge : MonoBehaviour
    {
        [Tooltip("How much amount transaction possible with help of this transiction bridge")]
        protected TransactionBridgeLimit transactionLimit;
        [Tooltip("How many time to delay in between transiction frame")]
        [SerializeField] protected Limiter timeIntervalLimit;

        [SerializeField] private bool useUserInput = false;
        [SerializeField,Tooltip("Where we will store all of the collection data based on Point ID")]
        protected TransactionContainer[] containers;
        private Coroutine routine; //store existin transiction routine
        protected virtual void Awake()
        {

            transactionLimit = GetComponent<TransactionBridgeLimit>();
        }

        /// <summary>
        /// Collecting routine process
        /// </summary>
        /// <param name="A">Source Point</param>
        /// <param name="B">Destination Point</param>
        /// <param name="delta">How much amound will be change</param>
        /// <returns></returns>
        private IEnumerator TransactionRoutine(TransactionContainer A, TransactionContainer B, int delta)
        {
            if (A == null || B == null)
                yield break;
            if(A.GetID != B.GetID)
                yield break;

            var useBridgeTransactionLimt = transactionLimit != null;

            var sign = 1;
            if (useBridgeTransactionLimt)
                if (A.transform == transactionLimit.transform)
                    sign = -1;
            
            while (true)
            {
                while(useBridgeTransactionLimt)
                {
                    if(useUserInput)
                        while (Input.GetMouseButton(0))
                            yield return null;

                    if (!A.willCrossLimit(-delta) && !B.willCrossLimit(delta) && transactionLimit.IsValidTransaction(delta * sign))
                    {
                        if (A.enabled && B.enabled)
                        { 
                            A.TransactFrom(-delta, B);
                            B.TransactFrom(delta, A);
                            transactionLimit.Transact(delta * sign);
                        }

                        if (timeIntervalLimit)
                            yield return new WaitForSeconds(timeIntervalLimit.GetCurrent);
                        else
                            yield return null;
                    }
                    else
                        yield return new WaitForSeconds(0.2f);

                    useBridgeTransactionLimt = transactionLimit != null;
                }
        
                while(!useBridgeTransactionLimt)
                {
                    if (useUserInput)
                        while (Input.GetMouseButton(0))
                            yield return null;

                    if (!A.willCrossLimit(-delta) && !B.willCrossLimit(delta))
                    {
                        if (A.enabled && B.enabled)
                        {
                            A.TransactFrom(-delta, B);
                            B.TransactFrom(delta, A);
                        }
                        if (timeIntervalLimit)
                            yield return new WaitForSeconds(timeIntervalLimit.GetCurrent);
                        else
                            yield return null;
                    }
                    else
                        yield return new WaitForSeconds(0.2f);
                }
            }
        }
        /// <summary>
        /// Stop existing transiction routine
        /// </summary>
        protected virtual void StopTransiction()
        {
            if (routine != null)
                StopCoroutine(routine);
        }
        /// <summary>
        /// Start tarnsiction routine
        /// </summary>
        /// <param name="A">Source Point</param>
        /// <param name="B">Destination Point</param>
        /// <param name="delta">How much amound will be change</param>
        protected virtual void StartTransiction(TransactionContainer A, TransactionContainer B, int delta = 1)
        {
            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(TransactionRoutine(A, B, delta));
        }
    }
}