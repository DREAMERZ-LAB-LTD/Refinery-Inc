using IdleArcade.Core;
using UnityEngine;

public class TransactorVisual : TransactionVisualCore
{
    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        var source = A.GetComponent<TransactionVisualCore>();
        if (source == null)
            return;

        var visualEntity = source.Pull_UsingLIFO(A.GetID);
        if (visualEntity)
            Push(visualEntity);
        
    }

    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        if (A == null) return;
        var destination = A.GetComponent<TransactionVisualCore>();
        if (destination == null) return;

        var visualEntity = Pull_UsingLIFO(A.GetID);
        if (visualEntity)
            destination.Push(visualEntity);
        
    }
}