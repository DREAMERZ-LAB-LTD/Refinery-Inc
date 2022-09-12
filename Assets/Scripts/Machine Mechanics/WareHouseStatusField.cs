using IdleArcade.Core;
using UnityEngine;
using TMPro;

public class WareHouseStatusField : ContainerStatusUI
{
    [SerializeField] private GameObject emptyIcon;

    protected override void OnContainerUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        statusText.text = startMessage + currnet  + endMessage;

        if (emptyIcon)
            emptyIcon.gameObject.SetActive(currnet <= 0);
    }
}
