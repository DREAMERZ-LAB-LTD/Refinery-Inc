using UnityEngine;
using TMPro;
namespace IdleArcade.Core
{
    public class ContainerStatusUI : MonoBehaviour
    {
        [SerializeField] protected TransactionContainer container;
        [SerializeField] protected TextMeshProUGUI statusText;
        [Header("Text Message")]
        [SerializeField] private string startMessage;
        [SerializeField] private string centertMessage = " / ";
        [SerializeField] private string endMessage;
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
            statusText.text = startMessage + currnet + centertMessage + max + endMessage;
        }
    }
}
