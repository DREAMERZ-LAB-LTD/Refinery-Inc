using System.Collections;
using UnityEngine;
using IdleArcade.Core;

public class ConvertingVisualFlow : MonoBehaviour, TransactionConverter.IConverter
{
    [SerializeField, Range(0f, 1f)] private float processingDelay = 0.5f;
    [SerializeField] private Entity convertedPrefab;
    [SerializeField] private Transform A;
    [SerializeField] private Transform B;
    private GameObject visualFlow;

    private void Awake()
    {
        visualFlow = Instantiate(convertedPrefab.gameObject, transform.position, transform.rotation, transform);
        visualFlow.SetActive(false);
    }
    public void OnConvertBegin(float delay)
    {
        StartCoroutine(MoveTo(A.position, B.position));

        IEnumerator MoveTo(Vector3 from, Vector3 to)
        {
            var processingTime = delay * processingDelay;
            if (processingTime > 0)
                yield return new WaitForSeconds(processingTime);

            var startTime = Time.time;
            var endTime = startTime + (delay - processingTime);
            float t = 0;
            visualFlow.SetActive(true);
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                visualFlow.transform.position = Vector3.Lerp(from, to, t);
                yield return null;
            }
            visualFlow.SetActive(false);
        }
    }

    public void OnConverted() { }
}
