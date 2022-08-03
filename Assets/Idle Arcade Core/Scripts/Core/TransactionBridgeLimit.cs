using UnityEngine;

namespace IdleArcade.Core
{ 
    public class TransactionBridgeLimit : Limiter
    {
        private int amount;

        public bool IsValidTransaction(int amount) => this.amount + amount <= GetCurrent && this.amount + amount >= range.x;

        /// <summary>
        /// Adding each of the transaction of the transaction bridge to track how many transact already done by this bridge
        /// </summary>
        /// <param name="delta">delta transcaction rate</param>
        /// <returns></returns>
        public bool Transact(int delta)
        {
            if (!IsValidTransaction(delta))
                return false;

            this.amount = Mathf.Clamp(this.amount + delta, (int) range.x, (int)range.y);
            return true;
        }
    }
}
