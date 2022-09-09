using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public class TransactionBridge : MonoBehaviour
    {
        [Tooltip("How much amount transaction possible with help of this transiction bridge")]
        protected TransactionBridgeLimit transactionLimit;
        [Tooltip("How many time to delay in between transiction frame")]
        [SerializeField] protected Limiter timeIntervalLimit;

        [SerializeField] private KeyCode interruptKey = KeyCode.None;
        [SerializeField,Tooltip("Where we will store all of the collection data based on Point ID")]
        private Coroutine routine; //store existin transiction routine
        protected virtual void Awake()
        {
            transactionLimit = GetComponent<TransactionBridgeLimit>();
        }

        /// <summary>
        /// Stop existing transiction routine
        /// </summary>
        public virtual void StopTransiction()
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
        public virtual void StartTransiction(TransactionContainer A, TransactionContainer B, int delta = 1)
        {
            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(TransactionRoutine(A, B, delta));
        }

        public virtual void StartTransiction(Containable from, Containable to, int delta = 1)
        {
            var fromCont = from.GetContainers;
            List<TransactionContainer> fromContainer = new List<TransactionContainer>();
            List<TransactionContainer> toContainer = new List<TransactionContainer>();

            for (int i = 0; i < fromCont.Length; i++)
            {
                var toCont = to.GetContainer(fromCont[i].GetID);
                if (toCont)
                {
                    fromContainer.Add(fromCont[i]);
                    toContainer.Add(toCont);
                }
            }
            if (fromContainer.Count == 0)
                return;

            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(TransactionRoutine(fromContainer, toContainer, delta));
        }

        /// <summary>
        /// Collecting routine process
        /// </summary>
        /// <param name="from">Source Point</param>
        /// <param name="to">Destination Point</param>
        /// <param name="delta">How much amound will be change</param>
        /// <returns></returns>
        private IEnumerator TransactionRoutine(TransactionContainer from, TransactionContainer to, int delta)
        {
            if (from == null || to == null)
                yield break;
            if(from.GetID != to.GetID)
                yield break;

            var useBridgeTransactionLimt = transactionLimit != null;

            var sign = 1;
            if (useBridgeTransactionLimt)
                if (from.transform == transactionLimit.transform)
                    sign = -1;
            
            while (true)
            {
                while(useBridgeTransactionLimt)
                {
                    if (!from.willCrossLimit(-delta) && !to.willCrossLimit(delta) && transactionLimit.IsValidTransaction(delta * sign))
                    {
                        while (Input.GetKey(interruptKey))
                            yield return null;

                        if (from.enabled && to.enabled)
                        { 
                            from.TransactFrom(-delta, to);
                            to.TransactFrom(delta, from);
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

                    if (!from.willCrossLimit(-delta) && !to.willCrossLimit(delta))
                    {
                        while (Input.GetKey(interruptKey))
                            yield return null;

                        if (from.enabled && to.enabled)
                        {
                            from.TransactFrom(-delta, to);
                            to.TransactFrom(delta, from);
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
      

        private IEnumerator TransactionRoutine(List<TransactionContainer> from, List<TransactionContainer> to, int delta)
        {
            var useBridgeTransactionLimt = transactionLimit != null;

            var sign = 1;
            if (useBridgeTransactionLimt)
                if (from[0].transform == transactionLimit.transform)
                    sign = -1;

            while (true)
            {
                while (useBridgeTransactionLimt)
                {
                    for (int i = 0; i < from.Count; i++)
                    {

                        var fromCont = from[i];
                        var toCont = to[i];
                        if (!fromCont.enabled || !toCont.enabled)
                            continue;

                        while (!fromCont.willCrossLimit(-delta) && !toCont.willCrossLimit(delta) && transactionLimit.IsValidTransaction(delta * sign))
                        {
                            while (Input.GetKey(interruptKey))
                                yield return null;

                            fromCont.TransactFrom(-delta, toCont);
                            toCont.TransactFrom(delta, fromCont);
                            transactionLimit.Transact(delta * sign);


                            if (timeIntervalLimit)
                                yield return new WaitForSeconds(timeIntervalLimit.GetCurrent);
                            else
                                yield return null;
                        }
                        
                    }
                    yield return new WaitForSeconds(0.2f);

                    useBridgeTransactionLimt = transactionLimit != null;
                }

                while (!useBridgeTransactionLimt)
                {
                    for (int i = 0; i < from.Count; i++)
                    {

                        var fromCont = from[i];
                        var toCont = to[i];
                        if (!fromCont.enabled || !toCont.enabled)
                            continue;

                        while (!fromCont.willCrossLimit(-delta) && !toCont.willCrossLimit(delta))
                        {
                            while (Input.GetKey(interruptKey))
                                yield return null;

                            fromCont.TransactFrom(-delta, toCont);
                            toCont.TransactFrom(delta, fromCont);

                            if (timeIntervalLimit)
                                yield return new WaitForSeconds(timeIntervalLimit.GetCurrent);
                            else
                                yield return null;
                        }
                    }
                    yield return new WaitForSeconds(0.2f);
                }
            }
        }
    }
}