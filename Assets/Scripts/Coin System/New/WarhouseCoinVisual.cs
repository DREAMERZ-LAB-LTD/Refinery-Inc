using UnityEngine;
using IdleArcade.Core;

public class WarhouseCoinVisual : TransactionVisualCore
{
    [SerializeField] private string prefabID;

    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        Entity coinVisual;
        Entity topEntity;
        for(int i = 0; i < delta; i++)
        {
            topEntity = PullNextElement_UsingFIFO(A.GetID);
            coinVisual = GameManager.instance.pullingSystem.Pull(prefabID);
            coinVisual.transform.position = topEntity ? topEntity.transform.position : transform.position;
            Push(coinVisual);
        }
    }

    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B) { }
}
