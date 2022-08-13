using UnityEngine;
using IdleArcade.Core;

public class TransactionSourceVisual : TransactionVisualCore
{
    [SerializeField] private Entity prefab;

    protected override void OnAdding(int delta, TransactionContainer A)
    {
        for (int i = 0; i < delta; i++)
        { 
            var spawnPoint = transform.position;
            var height = prefab.transform.localScale.y;
            spawnPoint.y += visualAmounts.Count * height + 1;
            var ability = SpawnEntity(spawnPoint, transform.rotation, transform);

            if(ability)
                Push(ability);
        }
    }

    protected Entity SpawnEntity(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var abilityPrefab = Instantiate(prefab.gameObject, position, rotation, parent);
        var ability = abilityPrefab.GetComponent<Entity>();
        if (ability == null)
            Destroy(abilityPrefab);

        return ability;
    }
}