using UnityEngine;
using IdleArcade.Core;

public class BuyerCar : NPC_CarBehaviour
{
    public TransactionVisualCore visual;

    protected override void OnExportSIde()
    {
        base.OnExportSIde();
      
        selfContainer.TransactFrom(-selfContainer.Getamount, selfContainer);
        selfContainer.enabled = false;
    }
}

