using UnityEngine;
using IdleArcade.Core;

public class WarhouseCoinVisual : TransactionVisualCore
{
    [SerializeField] private Entity coinPrefab;
    protected override void OnAdding(int delta, TransactionContainer A, TransactionContainer B)
    {
        for (int i = 0; i < delta; i++)
        {
            var position = transform.position;// A.transform.position;
            var ability = SpawnEntity(position, transform.rotation, transform);

            if (ability)
                Push(ability);
        }
    }

    protected override void OnRemoving(int delta, TransactionContainer A, TransactionContainer B)
    {
        for (int i = 0; i < Mathf.Abs(delta); i++)
        {
            var entity = Pull_UsingLIFO(B.GetID);
            if (entity)
                Destroy(entity.gameObject);
        }
    }


    protected Entity SpawnEntity(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var abilityPrefab = Instantiate(coinPrefab.gameObject, position, rotation, parent);
        var ability = abilityPrefab.GetComponent<Entity>();
        if (ability == null)
            Destroy(abilityPrefab);

        return ability;
    }
}
