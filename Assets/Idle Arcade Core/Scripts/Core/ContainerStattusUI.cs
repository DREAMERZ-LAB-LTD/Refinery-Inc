using UnityEngine;
using TMPro;
namespace IdleArcade.Core
{
    public class ContainerStattusUI : MonoBehaviour
    {
        [SerializeField] protected TransactionContainer container;
        [SerializeField] protected TextMeshProUGUI statusText;
        protected virtual void Awake()
        {
            container.OnChangedValue += OnContainerUpdate;
        }

        protected virtual void OnDestroy()
        { 
            container.OnChangedValue -= OnContainerUpdate;
        }


        protected virtual void OnContainerUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
        {
            statusText.text = currnet + " / " + max;
        }
    }
}
