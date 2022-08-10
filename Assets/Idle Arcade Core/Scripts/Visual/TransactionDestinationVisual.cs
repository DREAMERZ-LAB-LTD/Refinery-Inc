using IdleArcade.Core;

public class TransactionDestinationVisual : TransactionVisualCore
{
    protected override void OnRemoving(int delta, TransactionContainer A)
    {
        for (int i = 0; i < UnityEngine.Mathf.Abs(delta); i++)
        {
            var amount = Pull_UsingLIFO(A.GetID);
            Destroy(amount.gameObject);
        }
    }
}
