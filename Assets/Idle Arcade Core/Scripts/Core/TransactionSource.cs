using UnityEngine;

namespace IdleArcade.Core
{
    public class TransactionSource : MonoBehaviour
    {
        [SerializeField] protected TransactionContainer container;
        public TransactionContainer GetContainer => container;
    }
}