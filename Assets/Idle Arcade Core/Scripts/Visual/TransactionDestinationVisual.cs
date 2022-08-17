using IdleArcade.Core;

public class TransactionDestinationVisual : TransactionVisualCore
{
    protected override void OnAdding(int delta, TransactionContainer A, TransactionContainer B) { }

    protected override void OnRemoving(int delta, TransactionContainer A, TransactionContainer B)
    {
        delta = UnityEngine.Mathf.Abs(delta);
        for (int i = 0; i < delta; i++)
        {
            var amount = Pull_UsingLIFO(A.GetID);
            Destroy(amount.gameObject);
        }
    }
}
