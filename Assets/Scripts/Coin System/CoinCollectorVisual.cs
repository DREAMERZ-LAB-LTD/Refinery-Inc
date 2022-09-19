using IdleArcade.Core;
using System.Collections;
using UnityEngine;
public class CoinCollectorVisual : TransactionVisualCore
{
    [SerializeField, Range(0.01f, 0.5f)] private float destroyDelay = 0.2f;

    [SerializeField] private Entity coinPrefab;
    private GameObject coin = null;
    private void Start()
    {
        coin = Instantiate(coinPrefab.gameObject, transform.position, transform.rotation, transform);
        coin.SetActive(false);
    }
    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B) { }

    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        Transform toPoint = A.transform;
        StartCoroutine(MoveTo(coin, transform, toPoint));
    }


    IEnumerator MoveTo(GameObject coin, Transform fromPoint, Transform toPoint)
    {
        float starTime = Time.time;
        float endTime = starTime + 0.3f;
        float t;

        coin.SetActive(true);
        while (Time.time < endTime)
        {
            t = Mathf.InverseLerp(starTime, endTime, Time.time);
            coin.transform.position = Vector3.Lerp(fromPoint.position, toPoint.position, t);
            yield return null;
        }
        coin.SetActive(false);
    }
}
