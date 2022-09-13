using UnityEngine;

namespace IdleArcade.Core
{
    [RequireComponent(typeof(TransactionContainer))]

    public class TransactionCollector : Containable, TriggerDetector.ITriggerable
    {
        [SerializeField] protected TransactionBridge transactionBridge;

        /// <summary>
        /// Called when collider trigger enterd with source object
        /// Note : triigger musking by obect Tag.
        /// </summary>
        /// <param name="collider">source collider</param>
        public void OnEnter(Collider collider)
        {
            var sourcePoint = collider.GetComponent<TransactionSource>();
            if (sourcePoint == null) return;

            foreach (var storePoint in containers)
                if (sourcePoint.GetContainer.GetID == storePoint.GetID)
                {
                    transactionBridge.StartTransiction(sourcePoint.GetContainer, storePoint, 1);
                    break;
                }
        }
        /// <summary>
        /// Called when collider trigger exit from source object
        /// Note : triigger musking by obect Tag.
        /// </summary>
        /// <param name="collider">source collider</param>
        public void OnExit(Collider collider)
        {
            var point = collider.GetComponent<TransactionSource>();
            if (point == null) return;

            transactionBridge.StopTransiction();
        }
    }
}