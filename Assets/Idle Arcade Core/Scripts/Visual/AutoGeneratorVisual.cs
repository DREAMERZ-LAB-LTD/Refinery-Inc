using UnityEngine;
using IdleArcade.Core;

public class AutoGeneratorVisual : TransactionVisualCore
{
    [SerializeField] private Entity prefab;

    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
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
    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B) { }


    protected Entity SpawnEntity(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var abilityPrefab = Instantiate(prefab.gameObject, position, rotation, parent);
        var ability = abilityPrefab.GetComponent<Entity>();
        if (ability == null)
            Destroy(abilityPrefab);

        return ability;
    }
}