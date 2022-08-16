using System.Collections;
using UnityEngine;
namespace IdleArcade.Core
{
    public class TransactionConverter : MonoBehaviour
    {
        public interface IConverter
        {
            public void OnConvertBegin(float delay);
            public void OnConverted();

        }

        public delegate void OnConvert(float delay);
        public OnConvert OnConvertBegin;

        [SerializeField] protected Limiter timeintervallimit;

        [SerializeField, Tooltip("Source container where from the item convert to anoter")] 
        protected TransactionContainer from;
        [SerializeField, Tooltip("Destination container where item will stored after convert")] 
        protected TransactionContainer to;

        private Coroutine routine = null;
        private int delta = 1;

        protected IConverter[] converterResponses;

        protected virtual void Awake()
        {
            converterResponses = GetComponentsInChildren<IConverter>();
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
       

        protected virtual void OnProcessBegin(float delay) { }
        protected virtual void OnProcessEnd() { }

        /// <summary>
        /// Converting one item to another
        /// </summary>
        /// <returns></returns>
        private IEnumerator ConversionRoutine()
        {
            float delay = timeintervallimit ? timeintervallimit.GetCurrent : 0;
            while (from && to)
            {
                while (from.Getamount > 0)
                {
                    if (!from.willCrossLimit(-delta) && !to.willCrossLimit(delta))
                    {
                        OnProcessBegin(delay);
                        for (int i = 0; i < converterResponses.Length; i++)
                            converterResponses[i].OnConvertBegin(delay);
                        if(OnConvertBegin!= null)
                            OnConvertBegin.Invoke(delay);

                        from.TransactFrom(-delta, from);
                    }
                    if (delay > 0)
                        yield return new WaitForSeconds(delay);
                    else
                        yield return null;

                    to.Add(delta);
                    OnProcessEnd();
                    for (int i = 0; i < converterResponses.Length; i++)
                        converterResponses[i].OnConverted();
                }
                yield return new WaitForSeconds(1);
            }
        }
    }
}