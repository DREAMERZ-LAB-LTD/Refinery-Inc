using System.Collections;
using UnityEngine;
using IdleArcade.Core;

public class ConverterVisualFlow : MonoBehaviour, TransactionConverter.IConverter
{
    [SerializeField, Range(0f, 1f)] private float processingDelay = 0.5f;
    [Header("From Setup")]
    [SerializeField] private Entity fromPrefab;
    [SerializeField] private Transform fromA;
    [SerializeField] private Transform fromB;
    [Header("To Setup")]
    [SerializeField] private Entity convertedPrefab;
    [SerializeField] private Transform toA;
    [SerializeField] private Transform toB;
    private GameObject fromVisualElement;
    private GameObject toVisualElement;

    private void Awake()
    {
        if (fromPrefab)
        {
            fromVisualElement = Instantiate(fromPrefab.gameObject, fromA.position, fromA.rotation, transform);
            fromVisualElement.SetActive(false);
        }
        if (convertedPrefab)
        { 
            toVisualElement = Instantiate(convertedPrefab.gameObject, toA.position, toA.rotation, transform);
            toVisualElement.SetActive(false);
        }
    }
    public void OnConvertBegin(float delay)
    {
        StartCoroutine(MoveTo());

        IEnumerator MoveTo()
        {
            Vector3 fA = fromA ? fromA.position : Vector3.zero;
            Vector3 fB = fromB ? fromB.position: Vector3.zero;
            Vector3 tA = toA ? toA.position : Vector3.zero;
            Vector3 tB = toB ? toB.position : Vector3.zero;

            var processingTime = delay * processingDelay;

            var swipeDuration = delay - processingTime;
            var fromLength = Vector3.Distance(fA, fB);
            var toLength = Vector3.Distance(tA, tB);
            var totalPathLength = fromLength + toLength;

            var dt = Mathf.InverseLerp(0, totalPathLength, fromLength);
            var startTime = Time.time;
            var endTime = startTime + dt * swipeDuration;

            float t;
            if(fromVisualElement)
                fromVisualElement.SetActive(true);
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                if(fromVisualElement)
                    fromVisualElement.transform.position = Vector3.Lerp(fA, fB, t);
                yield return null;
            }
            if(fromVisualElement)
                fromVisualElement.SetActive(false);

            if (processingTime > 0)
                yield return new WaitForSeconds(processingTime);
            else
                yield return null;

            dt = Mathf.InverseLerp(fromLength, totalPathLength, toLength);
            startTime = Time.time;
            endTime = startTime + dt * swipeDuration;

            if(toVisualElement)
                toVisualElement.SetActive(true);
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                if(toVisualElement)
                    toVisualElement.transform.position = Vector3.Lerp(tA, tB, t);
                yield return null;
            }
            if(toVisualElement)
                toVisualElement.SetActive(false);
        }
    }

    public void OnConverted() { }
}
