using IdleArcade.Core;
using UnityEngine;

public class TransactorVisual : TransactionVisualCore
{
    protected override void OnAdding(int delta, TransactionContainer A, TransactionContainer B)
    {
        var source = A.GetComponent<TransactionVisualCore>();
        if (source == null)
            return;

        var visualEntity = source.Pull_UsingLIFO(A.GetID);
        if (visualEntity)
            Push(visualEntity);
        
    }

    protected override void OnRemoving(int delta, TransactionContainer A, TransactionContainer B)
    {
        var destination = A.GetComponent<TransactionVisualCore>();
        if (destination == null)
            return;

        var visualEntity = Pull_UsingLIFO(A.GetID);
        if (visualEntity)
            destination.Push(visualEntity);
        
    }
}