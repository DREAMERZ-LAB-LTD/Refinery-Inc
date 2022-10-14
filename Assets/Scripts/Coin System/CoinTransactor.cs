using IdleArcade.Core;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CoinTransactor : TransactionBridge, TriggerDetector.ITriggerable
{
    [SerializeField] private UnityEvent OnCollectBegin;
    [SerializeField] private UnityEvent OnSpentBegin;

    [Tooltip("Where we will store all of the collection data based on Point ID")]
    private TransactionContainer[] containers;
    protected override void Awake()
    {
        base.Awake();
        //assign all of the points to use for store all of the collecting data
        containers = GetComponents<TransactionContainer>();
    }

    public void OnEnter(Collider collider)
    {
        var destinationPoint = collider.GetComponent<TransactionDestination>();
        if (destinationPoint)
        { 
            OnDistribute(destinationPoint);
            return;
        }

        var coinSource = collider.GetComponent<TransactionSource>();
        if (coinSource)
            OnCoinCollect(coinSource);
    }

    private void OnCoinCollect(TransactionSource coinSource)
    {
        TransactionContainer playerCoinContainer = null;
        TransactionContainer coinSourceContainer = null;
    
        for (int i = 0; i < containers.Length; i++)
        {
            playerCoinContainer = containers[i];
            coinSourceContainer = coinSource.GetContainer(playerCoinContainer.GetID);
            if (coinSourceContainer)
                if (coinSourceContainer.Getamount > 0)
                    break;
        }

        if (coinSourceContainer && playerCoinContainer)
        { 
            StartTransiction(coinSourceContainer, playerCoinContainer, coinSourceContainer.Getamount);
            OnCollectBegin.Invoke();
        }
    }


    /// <summary>
    /// Called when collider trigger exit from destination object
    /// </summary>
    /// <param name="collider">destination collider</param>
    public void OnExit(Collider collider)
    {
        StopTransiction();
    }

    private void OnDistribute(TransactionDestination destinationPoint)
    {
        var destinationContainer = destinationPoint.GetContainer(containers[0].GetID);//////////////////////////////////////////////////////////
        if (destinationContainer == null)
            return;

        var fps = Application.targetFrameRate > 0 ? Application.targetFrameRate : 60;
        foreach (var storePoint in containers)
            if (destinationContainer.GetID == storePoint.GetID)
            {
                var target = destinationContainer.GetMax;
                if (target <= storePoint.Getamount)
                {
                    var unlockable = destinationContainer as UnlockableContainer;
                    if (unlockable)
                    {
                        if (!unlockable.isValidToUnlock())
                            return;
                    }
                    var frameLength = 1 / (float)fps;
                    target *= frameLength;
                    StopAllCoroutines();
                    StartCoroutine(TransactionRoutine(storePoint, destinationContainer, (int)target));
                }
                break;
            }
    }


    private IEnumerator TransactionRoutine(TransactionContainer A, TransactionContainer B, int delta)
    {
        if (A == null || B == null)
            yield break;
        if (A.GetID != B.GetID)
            yield break;

        if (!B.isFilledUp)
            OnSpentBegin.Invoke();

        while (!A.isEmpty && !B.isFilledUp)
        {
            while (Input.GetKey(interruptKey))
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
