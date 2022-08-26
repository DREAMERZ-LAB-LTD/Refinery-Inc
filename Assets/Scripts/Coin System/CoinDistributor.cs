using IdleArcade.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDistributor : MonoBehaviour, TriggerDetector.ITriggerable
{
    [SerializeField] private bool useUserInput = false;

    [Tooltip("Where we will store all of the collection data based on Point ID")]
    private TransactionContainer[] storePoints;
    private Coroutine routine; //store existin transiction routine
    private void Awake()
    {
        //assign all of the points to use for store all of the collecting data
        storePoints = GetComponents<TransactionContainer>();
    }

    public void OnEnter(Collider collider)
    {
        var destinationPoint = collider.GetComponent<TransactionDestination>();
        if (destinationPoint == null) return;

        var destinationContainer = destinationPoint.GetContainer;
        if (destinationContainer == null)
            return;

        foreach (var storePoint in storePoints)
            if (destinationContainer.GetID == storePoint.GetID)
            {
                var target = destinationContainer.GetMax;
                if (target <= storePoint.Getamount)
                {
                    var frameLength = 1 / 60f;
                    target *= frameLength;
                    StopAllCoroutines();
                    StartCoroutine(TransactionRoutine(storePoint, destinationContainer, (int)target));
                }
                break;
            }
    }

    /// <summary>
    /// Called when collider trigger exit from destination object
    /// </summary>
    /// <param name="collider">destination collider</param>
    public void OnExit(Collider collider)
    {

    }


    private IEnumerator TransactionRoutine(TransactionContainer A, TransactionContainer B, int delta)
    {
        if (A == null || B == null)
            yield break;
        if (A.GetID != B.GetID)
            yield break;

        while (true)
        {
            if (useUserInput)
                while (Input.GetMouseButton(0))
                    yield return null;

            if (!A.willCrossLimit(-delta) && !B.willCrossLimit(delta))
            {
                if (A.enabled && B.enabled)
                {
                    A.TransactFrom(-delta, B);
                    B.TransactFrom(delta, A);
                }
                yield return null;
            }
            else
            {
                delta = (int)B.GetMax - B.Getamount;
                if (delta > 0)
                {
                    A.TransactFrom(-delta, B);
                    B.TransactFrom(delta, A);
                }
                yield return new WaitForSeconds(0.2f);
            }

        }
    }
}
