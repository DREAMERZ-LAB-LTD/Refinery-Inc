using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    [RequireComponent(typeof(TransactionBridge))]
    public class TransactionDistributor : Containable, TriggerDetector.ITriggerable
    {

        [SerializeField] protected TransactionBridge transactionBridge;
        /// <summary>
        /// Called when collider trigger enterd with destination object
        /// Note : triigger musking by obect Tag.
        /// </summary>
        /// <param name="collider">destination collider</param>
        public void OnEnter(Collider collider)
        {
            var destinationPoint = collider.GetComponent<TransactionDestination>();
            if (destinationPoint == null) return;

     
            transactionBridge.StartTransiction(this, destinationPoint, 1);
        }

        /// <summary>
        /// Called when collider trigger exit from destination object
        /// </summary>
        /// <param name="collider">destination collider</param>
        public void OnExit(Collider collider)
        {
            var point = collider.GetComponent<TransactionContainer>();
            if (point == null) return;

            transactionBridge.StopTransiction();
        }
    }
}