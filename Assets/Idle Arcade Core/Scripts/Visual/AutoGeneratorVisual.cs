using UnityEngine;
using IdleArcade.Core;

public class AutoGeneratorVisual : TransactionVisualCore
{
    [SerializeField] private Entity prefab;
    [SerializeField] private string prefabID;
    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        Entity prefab;

        for (int i = 0; i < delta; i++)
        { 
            prefab = GameManager.instance.pullingSystem.Pull(prefabID);
            var height = prefab.transform.localScale.y;
            var spawnPoint = transform.position;
            spawnPoint.y += visualAmounts.Count * height + 1;

            prefab.transform.SetPositionAndRotation(spawnPoint, transform.rotation);
            prefab.transform.parent = transform;

            if (prefab)
                Push(prefab);
        }
    }
    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B) { }


}