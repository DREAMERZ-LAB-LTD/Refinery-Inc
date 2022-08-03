using UnityEngine;
using IdleArcade.Core;

public class TransactionSourceVisual : TransactionVisualCore
{
    [SerializeField] private Entity prefab;

    protected override void OnAdding(int delta, TransactionContainer A)
    {
        var rotation = transform.rotation;
        var spawnPoint = transform.position;
        var height = prefab.transform.localScale.y;
        spawnPoint.y += visualAmounts.Count * height + 1;
        var abilityPrefab = Instantiate(prefab.gameObject, spawnPoint, rotation, transform);

        var ability = abilityPrefab.GetComponent<Entity>();
        Push(ability);
    }

    protected override void OnRemoving(int delta, TransactionContainer A)
    {
        Rearrange();
    }
}