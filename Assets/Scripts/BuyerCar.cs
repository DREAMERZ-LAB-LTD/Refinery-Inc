using UnityEngine;
using IdleArcade.Core;

public class BuyerCar : NPC_CarBehaviour
{
    public TransactionVisualCore visual;

    protected override void OnExportSIde()
    {
        base.OnExportSIde();
        Debug.Log("Exported " + name);

        selfContainer.TransactFrom(-selfContainer.Getamount, selfContainer);
    }

}
