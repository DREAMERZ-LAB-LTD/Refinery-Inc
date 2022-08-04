using System.Collections;
using UnityEngine;
namespace IdleArcade.Core
{
    public class TransactionConverter : MonoBehaviour
    {
        [SerializeField] private string timeIntervalLimitID;
        protected Limiter timeintervallimit;

        [SerializeField, Tooltip("Source container where from the item convert to anoter")] 
        protected TransactionContainer from;
        [SerializeField, Tooltip("Destination container where item will stored after convert")] 
        protected TransactionContainer to;

        private Coroutine routine = null;
        private int delta = 1;

        protected virtual void Awake()
        {
            var limits = GetComponents<Limiter>();
            foreach (var limit in limits)
                if (limit.GetID == timeIntervalLimitID)
                {
                    timeintervallimit = limit;
                    break;
                }
        }

        protected virtual void OnEnable()
        {
            if (routine != null)
                StopCoroutine(routine);
            routine = StartCoroutine(ConversionRoutine());
        }
        protected virtual void OnDisable()
        {
            if (routine != null)
                StopCoroutine(routine);
        }
       

        protected virtual void OnProcessBegin() { }
        protected virtual void OnProcessEnd() { }

        /// <summary>
        /// Converting one item to another
        /// </summary>
        /// <returns></returns>
        private IEnumerator ConversionRoutine()
        {
            while (from && to)
            {
                while (from.Getamount > 0)
                {
                    if (!from.willCrossLimit(-delta) && !to.willCrossLimit(delta))
                    { 
                        OnProcessBegin();
                        from.TransactFrom(-delta, from);
                    }
                    if (timeintervallimit)
                        yield return new WaitForSeconds(timeintervallimit.GetCurrent);
                    else
                        yield return null;


                    to.Add(delta);
                    OnProcessEnd();
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}