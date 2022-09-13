using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleArcade.Core;

public class AutoGenerator : MonoBehaviour
{
    [SerializeField] protected TransactionContainer container;
    [SerializeField] protected Limiter timeIntervalLimit;
    protected const int delta = 1;
    protected Coroutine routine;

    private IEnumerator GeneratorRoutine()
    {
        while (container)
        {
            if (timeIntervalLimit)
                yield return new WaitForSeconds(timeIntervalLimit.GetCurrent);
            else
                yield return null;

            if (!container.isFilledUp)
                container.Add(delta);
        }
    }

    /// <summary>
    /// stop the current generator routine
    /// </summary>
    protected void StopRoutine()
    {
        if (routine != null)
            StopCoroutine(routine);
    }

    protected virtual void OnEnable()
    {
        StopRoutine();
        routine = StartCoroutine(GeneratorRoutine());
    }
    protected virtual void OnDisable()
    {
        StopRoutine();
    }
}
