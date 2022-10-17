using IdleArcade.Core;
using System.Collections;
using UnityEngine;
public class CoinTransactorVisual : TransactionVisualCore
{
    [SerializeField, Range(0.01f, 0.5f)] private float destroyDelay = 0.2f;

    [SerializeField] private Entity coinPrefab;
   

    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B) 
    {
        if (A == null)
            return;

        var coinVisual = A.GetComponent<TransactionVisualCore>();
    
        float startinDelay = 0;
        float dt = Application.targetFrameRate > 0 ? 1 / (float)Application.targetFrameRate : 1 / 60f; 
        for (int i = 0; i < delta; i++)
        {
            var coin = coinVisual.Pull_UsingFIFO(A.GetID);
            StartCoroutine(MoveTo(coin, coin.transform.position, transform, 0.2f, startinDelay));
            startinDelay += dt;
        }

        IEnumerator MoveTo(Entity coin, Vector3 fromPoint, Transform to, float flowDuration, float startingDelay = 0)
        {
            if (startingDelay > 0)
                yield return new WaitForSeconds(startingDelay);

            float starTime = Time.time;
            float endTime = starTime + flowDuration;
            float t;
            Vector3 initialSale = coin.transform.localScale;
            Vector3 targetSale = coin.transform.localScale * 0.3f;
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(starTime, endTime, Time.time);
                coin.transform.position = Vector3.Lerp(fromPoint, to.position, t);
                coin.transform.localScale = Vector3.Lerp(initialSale, targetSale, t);
                yield return null;
            }
            coin.transform.localScale = initialSale;
            GameManager.instance.pullingSystem.Push(coin);
        }
    }



    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        if (A == null)
            return;

        var coin = GameManager.instance.pullingSystem.Pull(A.GetID);
        StartCoroutine( MoveTo(coin, B.transform, A.transform, .5f));

        IEnumerator MoveTo(Entity coin, Transform from, Transform to, float flowDuration, float startingDelay = 0)
        {
            if (startingDelay > 0)
                yield return new WaitForSeconds(startingDelay);

            float starTime = Time.time;
            float endTime = starTime + flowDuration;
            float t;
            Vector3 initialSale = coin.transform.localScale;
            Vector3 targetSale = coin.transform.localScale * 0.3f;
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(starTime, endTime, Time.time);
                coin.transform.position = Vector3.Slerp(from.position, to.position, t);
                coin.transform.rotation = Quaternion.Slerp(from.rotation, to.rotation, t);
                coin.transform.localScale = Vector3.Lerp(targetSale, initialSale, t);
                yield return null;
            }
            coin.transform.localScale = initialSale;
            GameManager.instance.pullingSystem.Push(coin);
        }
    }


}
