using UnityEngine;

namespace IdleArcade.Core
{
    [RequireComponent(typeof(TransactionContainer))]
    public class TransactionDestination : MonoBehaviour
    {
        [SerializeField] private string containerID;
        protected TransactionContainer container;
        public TransactionContainer GetContainer => container;

        protected virtual void Awake()
        {
            var containers = GetComponents<TransactionContainer>();
            for (int i = 0; i < containers.Length; i++)
                if (containerID == containers[i].GetID)
                {
                    container = containers[i];
                    break;
                }
        }
    }
}
