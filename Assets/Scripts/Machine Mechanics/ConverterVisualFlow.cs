using System.Collections;
using UnityEngine;
using IdleArcade.Core;

public class ConverterVisualFlow : MonoBehaviour, TransactionConverter.IConverter
{
    [SerializeField, Range(0f, 1f)] private float processingDelay = 0.5f;
    [SerializeField] private Entity fromPrefab;
    [SerializeField] private Entity convertedPrefab;
    [SerializeField] private Transform A;
    [SerializeField] private Transform B;
    [SerializeField] private Transform C;
    private GameObject fromVisualElement;
    private GameObject toVisualElement;

    private void Awake()
    {
        fromVisualElement = Instantiate(fromPrefab.gameObject, A.position, A.rotation, transform);
        fromVisualElement.SetActive(false);
        toVisualElement = Instantiate(convertedPrefab.gameObject, B.position, B.rotation, transform);
        toVisualElement.SetActive(false);
    }
    public void OnConvertBegin(float delay)
    {
        StartCoroutine(MoveTo(A.position, B.position, C.position));

        IEnumerator MoveTo(Vector3 A,Vector3 B, Vector3 C)
        {
            var processingTime = delay * processingDelay;

            var swipeDuration = delay - processingTime;
            var totalPathLength = (C - A).magnitude;

            var pathLength = (A - B).magnitude;
            var dt = Mathf.InverseLerp(0, totalPathLength, pathLength);
            var startTime = Time.time;
            var endTime = startTime + dt * swipeDuration;

            float t;
            fromVisualElement.SetActive(true);
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                fromVisualElement.transform.position = Vector3.Lerp(A, B, t);
                yield return null;
            }
            fromVisualElement.SetActive(false);

            if (processingTime > 0)
                yield return new WaitForSeconds(processingTime);
            else
                yield return null;

            pathLength = (B - C).magnitude;
            dt = Mathf.InverseLerp(0, totalPathLength, pathLength);
            startTime = Time.time;
            endTime = startTime + dt * swipeDuration;

            toVisualElement.SetActive(true);
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                toVisualElement.transform.position = Vector3.Lerp(B, C, t);
                yield return null;
            }
            toVisualElement.SetActive(false);
        }
    }

    public void OnConverted() { }
}
