using UnityEngine;
using TMPro;

namespace IdleArcade.Core
{ 
    public class TransactionBridgeLimit : UpgradeableLimiter
    {
        private int amount;
        [Header("UI Status"), SerializeField] 
        private TextMeshProUGUI text;

        private void Awake()
        {
            UpdateStatus();
        }

        protected override void OnUpgrade(float t)
        {
            base.OnUpgrade(t);
            UpdateStatus();
        }

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

            amount = Mathf.Clamp(amount + delta, (int) range.x, (int)range.y);
            UpdateStatus();

            return true;
        }

        private void UpdateStatus()
        {
            if (text)
                text.text = amount + " / " + GetCurrent;
        }
    }
}
