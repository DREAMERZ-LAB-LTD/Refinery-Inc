using IdleArcade.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinTransactorVisual : TransactionVisualCore
{
    [SerializeField, Range(0.01f, 0.5f)] private float destroyDelay = 0.2f;

    [SerializeField] private Entity coinPrefab;
   

    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B) 
    {
        var coinVisual = A.GetComponent<TransactionVisualCore>();
        var fromPoint = transform.position;
        List<Entity> entitys = new List<Entity>();
        for (int i = 0; i < delta; i++)
            entitys.Add(coinVisual.Pull_UsingFIFO(A.GetID));

        float startinDelay = 0;
        float dt = Application.targetFrameRate > 0 ? 1 / (float)Application.targetFrameRate : 1 / 60f; 
        for (int i = entitys.Count -1; i >= 0 ; i--)
        {
            var coin = entitys[i];
            StartCoroutine(MoveTo(coin, coin.transform.position, fromPoint, 0.2f, startinDelay));
            startinDelay += dt;
        }

        IEnumerator MoveTo(Entity coin, Vector3 fromPoint, Vector3 toPoint, float flowDuration, float startingDelay = 0)
        {
            if (startingDelay > 0)
                yield return new WaitForSeconds(startingDelay);

            float starTime = Time.time;
            float endTime = starTime + flowDuration;
            float t;

            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(starTime, endTime, Time.time);
                coin.transform.position = Vector3.Lerp(fromPoint, toPoint, t);
                yield return null;
            }
            GameManager.instance.pullingSystem.Push(coin);
        }
    }



    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        var coin = GameManager.instance.pullingSystem.Pull(A.GetID);
        StartCoroutine( MoveTo(coin, B.transform, A.transform, .5f));

        IEnumerator MoveTo(Entity coin, Transform from, Transform to, float flowDuration, float startingDelay = 0)
        {
            if (startingDelay > 0)
                yield return new WaitForSeconds(startingDelay);

            float starTime = Time.time;
            float endTime = starTime + flowDuration;
            float t;

            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(starTime, endTime, Time.time);
                coin.transform.position = Vector3.Slerp(from.position, to.position, t);
                coin.transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);

                yield return null;
            }
            GameManager.instance.pullingSystem.Push(coin);
        }
    }


}
