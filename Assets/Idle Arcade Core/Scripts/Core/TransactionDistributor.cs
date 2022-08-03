using UnityEngine;

namespace IdleArcade.Core
{
    public class TransactionDistributor : TransactionBridge, TriggerDetector.ITriggerable
    {
        /// <summary>
        /// Called when collider trigger enterd with destination object
        /// Note : triigger musking by obect Tag.
        /// </summary>
        /// <param name="collider">destination collider</param>
        public void OnEnter(Collider collider)
        {
            var destinationPoint = collider.GetComponent<TransactionDestination>();
            if (destinationPoint == null) return;

            var destinationContainer = destinationPoint.GetContainer;
            if (destinationContainer == null)
                return;

            foreach (var storePoint in storePoints)
                if (destinationContainer.GetID == storePoint.GetID)
                {
                    StartTransiction(storePoint, destinationContainer, 1);
                    break;
                }
        }

        /// <summary>
        /// Called when collider trigger exit from destination object
        /// </summary>
        /// <param name="collider">destination collider</param>
        public void OnExit(Collider collider)
        {
            var point = collider.GetComponent<TransactionContainer>();
            if (point == null) return;

            StopTransiction();
        }
    }
}