using IdleArcade.Core;
using UnityEngine;

public class TransactorVisual : TransactionVisualCore
{
    protected override void OnAdding(int delta, TransactionContainer A)
    {
        var source = A.GetComponent<TransactionSourceVisual>();
        if (source == null)
            return;

        var visualEntity = source.Pull_UsingLIFO(A.GetID);
        if (visualEntity)
        {
            Push(visualEntity);
            visualEntity.transform.parent = transform;
        }


        Rearrange();
    }

    protected override void OnRemoving(int delta, TransactionContainer A)
    {
        var destination = A.GetComponent<TransactionDestinationVisual>();
        if (destination == null)
            return;

        var visualEntity = Pull_UsingLIFO(A.GetID);
        if (visualEntity)
        {
            destination.Push(visualEntity);
            visualEntity.transform.parent = destination.transform;
        }

        Rearrange();
    }
}