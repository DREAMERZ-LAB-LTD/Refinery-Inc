using UnityEngine;
using UnityEngine.Events;

namespace IdleArcade.Core
{
    public class TransactionContainer : Entity
    {
        public delegate void Status();
        public delegate void ChangedValue(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B);
        public ChangedValue OnChangedValue;//Invoked when point ability value updated successfully
        public Status OnEmpty;//invoked when point ability value updated successfully and reach to highest value of ability limiter
        public Status OnFilled;//invoked when point ability value updated successfully and reach to lowest value of ability limiter

      
        [SerializeField, Tooltip("Current amount of this transaction point")]
        protected int m_amount;

        [SerializeField, Tooltip("Which limitter will work to limit clamp the capacity of this container")]
        public Limiter amountLimit;

        [Header("Callback Events")]
        [SerializeField] protected UnityEvent m_OnAdding;
        [SerializeField] protected UnityEvent m_OnRemoving;
        [SerializeField] protected UnityEvent m_OnFilledUp;
        [SerializeField] protected UnityEvent m_OnEmpty;

        /// <summary>
        /// return amount limiter of this point
        /// </summary>
        protected Limiter GetAmountLimit => amountLimit;

        public float GetMax
        {
            get
            {
                if (amountLimit)
                    return amountLimit.GetCurrent;

                return Mathf.Infinity;
            }
            
        }
        public int Getamount => m_amount;
        /// <summary>
        /// return true if transaction amount reached to maximum value of transaction amount limiter
        /// </summary>
        public bool isFilledUp
        {
            get 
            {
                if (amountLimit)
                    return  m_amount == (int)amountLimit.GetCurrent;
                
                return false;
            }
        }
        /// <summary>
        /// return true if transaction amount reached to minimum value of transaction amount limiter
        /// </summary>
        public bool isEmpty
        {
            get
            { 
                if(amountLimit)
                   return m_amount == amountLimit.GetRange.x;

                return m_amount == 0;
            }
        }

        /// <summary>
        /// return true if the result of the sumation will corssed the limit
        /// </summary>
        /// <param name="deltaAmount"></param>
        /// <returns></returns>
        public bool willCrossLimit(int deltaAmount)
        {
            if (deltaAmount > 0)
            {
                if(amountLimit)
                    if (m_amount + deltaAmount > amountLimit.GetCurrent)
                        return true;
            }
            else
            {
                if (amountLimit)
                {
                    if (m_amount + deltaAmount < amountLimit.GetRange.x)
                        return true;
                }
                else if (m_amount + deltaAmount < 0)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Clamped transaction amount based on transaction amount limiter
        /// </summary>
        protected virtual void ApplyLimit()
        {
            if (amountLimit)
                m_amount = Mathf.Clamp(m_amount, (int)amountLimit.GetRange.x, (int)amountLimit.GetCurrent);
            else if (m_amount < 0)
                m_amount = 0;
        }
      
        /// <summary>
        /// Initiaize all of the required peoperty
        /// </summary>
        protected virtual void Awake()
        {
            Add(0);//update all of the UI just like initialize
        }

        /// <summary>
        ///  Called when transaction amount value updated successfully and reached to highest value of transaction amount limiter
        /// </summary>
        protected virtual void OnFilledUp() { }
        /// <summary>
        /// Called when transaction amount value updated successfully and reached to lowest value of transaction amount limiter
        /// </summary>
        protected virtual void OnGetEmpty() { }

        /// <summary>
        /// Called each of the changes of transaction amount than Invoke all of the call back functions, delegates and events 
        /// </summary>
        /// <param name="delta">Delta change amount</param>
        /// <param name="A">Where from transaction occur</param>
        private void UpdateChangesResponse(int delta, TransactionContainer A)
        {
            if (isFilledUp)
            {
                OnFilledUp();
                if (OnFilled != null)
                    OnFilled.Invoke();

                m_OnFilledUp.Invoke();
            }
            if (isEmpty)
            {
                OnGetEmpty();
                if (OnEmpty != null)
                    OnEmpty.Invoke();

                m_OnEmpty.Invoke();
            }

            if (OnChangedValue != null)
            { 
                if(amountLimit)
                    OnChangedValue.Invoke(delta, m_amount, (int)amountLimit.GetCurrent, GetID, A, this);
                else
                    OnChangedValue.Invoke(delta, m_amount, m_amount, GetID, A, this);
            }

            if (delta > 0)
                m_OnAdding.Invoke();
            if (delta < 0)
                m_OnRemoving.Invoke();
        }


        /// <summary>
        /// Add delta amount into the transaction amount
        /// Positive (+) delta will increase transaction amount,
        /// Negetive (-) deltawill decrease transaction amount
        /// Note : Transaction amount range clamped with transaction amount limiter
        /// </summary>
        /// <param name="delta">Change amount</param>
        /// <returns></returns>
        public virtual bool Add(int delta)
        {
            if (willCrossLimit(delta)) return false;

            m_amount += delta;
            ApplyLimit();

            UpdateChangesResponse(delta, null);

            return true;
        }


        /// <summary>
        /// Add delta amount into the transaction amount
        /// Positive (+) delta will increase transaction amount
        /// Negetive (-) deltawill decrease transaction amount
        /// Note : Transaction amount range clamped with transaction amount limiter
        /// Return true if transaction amount value update successfully otherwise return false
        /// </summary>
        /// <param name="delta">Change Amount</param>
        /// <param name="A">Where from transaction occur</param>
        /// <returns></returns>
        public virtual bool TransactFrom(int delta, TransactionContainer A)
        {
            if (GetID != A.GetID) return false;
            if (willCrossLimit(delta)) return false;

            m_amount += delta;
            ApplyLimit();

            UpdateChangesResponse(delta, A);
            return true;
        }
    }
}