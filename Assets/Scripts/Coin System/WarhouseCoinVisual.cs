using UnityEngine;
using IdleArcade.Core;

public class WarhouseCoinVisual : TransactionVisualCore
{
    [SerializeField] private string prefabID;

    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        for(int i = 0; i < delta; i++)
            if(i == delta -1)
                Push(GameManager.instance.pullingSystem.Pull(prefabID));
            else
                Push(GameManager.instance.pullingSystem.Pull(prefabID), false);
    }

    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B) { }
}
