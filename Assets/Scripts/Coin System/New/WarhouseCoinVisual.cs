using UnityEngine;
using IdleArcade.Core;
using System.Collections.Generic;
using System.Collections;

public class WarhouseCoinVisual : TransactionVisualCore
{
    [SerializeField] private int initialPullAmount = 200;
    [SerializeField] private Entity coinPrefab;
    Stack<Entity> pullableCoins = new Stack<Entity>();
    protected override void Awake()
    {
        base.Awake();

        Entity coinVisual;
        for (int i = 0; i < initialPullAmount; i++)
        {
            coinVisual = SpawnEntity(transform.position, transform.rotation, transform);
            if (coinVisual)
            { 
                pullableCoins.Push(coinVisual);
                coinVisual.gameObject.SetActive(false);
            }
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


    IEnumerator Move(Entity coinVisual, Transform fromPoint, Transform toPoint, float duration)
    {
        float starTime = Time.time;
        float endTime = starTime + duration;
        float t;

        while (Time.time < endTime)
        {
            t = Mathf.InverseLerp(starTime, endTime, Time.time);
            coinVisual.transform.position = Vector3.Lerp(fromPoint.position, toPoint.position, t);
            yield return null;
        }
        pullableCoins.Push(coinVisual);
        coinVisual.gameObject.SetActive(false);
    }

    protected override void OnAdding(int delta, TransactionContainer A, TransactionContainer B)
    {
        var position = A.transform.position;
        var rotation = transform.rotation;
        Entity coinVisual;
        while (pullableCoins.Count > 0 && delta > 0)
        {
            delta--;
            coinVisual = pullableCoins.Pop();
            coinVisual.gameObject.SetActive(true);
            Push(coinVisual);
        }

        for (int i = 0; i < delta; i++)
        {
            coinVisual = SpawnEntity(position, rotation, transform);

            if (coinVisual)
                Push(coinVisual);
        }
    }

    protected override void OnRemoving(int delta, TransactionContainer A, TransactionContainer B)
    {
        Entity coinVisual;
        for (int i = 0; i < Mathf.Abs(delta); i++)
        {
            coinVisual = Pull_UsingLIFO(B.GetID);
            if (coinVisual)
            {
                StartCoroutine(Move(coinVisual, B.transform, A.transform, 0.5f));
            }
        }
    }


}
