using UnityEngine;
using IdleArcade.Core;
using System.Collections;

public class WarhouseCoinVisual : TransactionVisualCore
{
    [SerializeField] private string prefabID;
   
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
        GameManager.instance.pullingSystem.Push(coinVisual);
    }

    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        var position = transform.position;
        var rotation = transform.rotation;
        Entity coinVisual;
        for(int i = 0; i < delta; i++)
        {
            coinVisual = GameManager.instance.pullingSystem.Pull(prefabID);
            coinVisual.transform.SetPositionAndRotation(position, rotation);
            Push(coinVisual);
        }
    }

    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        Entity coinVisual;
        for (int i = 0; i < Mathf.Abs(delta); i++)
        {
            coinVisual = Pull_UsingLIFO(B.GetID);
            if (coinVisual)
                StartCoroutine(Move(coinVisual, B.transform, A.transform, 0.5f));
        }
    }


}
