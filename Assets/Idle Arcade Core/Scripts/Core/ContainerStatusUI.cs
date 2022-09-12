using UnityEngine;
using TMPro;
using System.Collections;
namespace IdleArcade.Core
{
    public class ContainerStatusUI : MonoBehaviour
    {
        [SerializeField] private string upgradebaleID;
        [SerializeField] protected TransactionContainer container;
        [SerializeField] protected TextMeshProUGUI statusText;
        [Header("Text Message")]
        [SerializeField] protected string startMessage;
        [SerializeField] protected string centertMessage = " / ";
        [SerializeField] protected string endMessage;

        protected virtual void Start()
        {
            container.OnChangedValue += OnContainerUpdate;
            if (upgradebaleID != string.Empty)
            {
                var upgradeableData = UpgradeSystem.instance.GetDataField(upgradebaleID);
                if (upgradeableData != null)
                    upgradeableData.OnChanged += OnUpgrade;
            }

            StartCoroutine(InitialRefresh());
            IEnumerator InitialRefresh()
            {
                yield return null;
                container.Add(0);
            }
        }

        protected virtual void OnDestroy()
        { 
            container.OnChangedValue -= OnContainerUpdate;
            if (upgradebaleID != string.Empty)
            {
                if (UpgradeSystem.instance)
                { 
                    var upgradeableData = UpgradeSystem.instance.GetDataField(upgradebaleID);
                    if (upgradeableData != null)
                        upgradeableData.OnChanged -= OnUpgrade;
                }
            }
        }

        private void OnUpgrade(float t)
        { 
            container.Add(0);//just refresh all of the listner to upto date the updated status
        }

        protected virtual void OnContainerUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
        {
            statusText.text = startMessage + currnet + centertMessage + max + endMessage;
        }
    }
}
