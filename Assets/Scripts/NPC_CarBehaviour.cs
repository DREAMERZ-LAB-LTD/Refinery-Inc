using System.Collections;
using UnityEngine;
using SWS;
using IdleArcade.Core;
using UnityEngine.Events;

public class NPC_CarBehaviour : MonoBehaviour
{
    public splineMove mover;
    public int importPoint;
    public int exportPoint;

    public TransactionContainer selfContainer;

    public UnityEvent onImportPoint;
    public UnityEvent onExportPoint;
    public UnityEvent onTheWay;

    private void OnEnable()
    {
        mover.movementChangeEvent += OnChangePoint;
    }
    private void OnDisable()
    {
        mover.movementChangeEvent -= OnChangePoint;
        StopAllCoroutines();
    }




    private void OnChangePoint(int index)
    {
        Debug.Log("Point At=" + index);
        StopAllCoroutines();

        var insideImport = importPoint == index;
        var insideExport = exportPoint == index;

        if (insideImport)
        { 
            Debug.Log("Point A");

            selfContainer.enabled = true;
            StartCoroutine(OnImportSide());
            onImportPoint.Invoke();
        }  
        if (insideExport)
        { 
            Debug.Log("Point B");

            selfContainer.enabled = true;
            StartCoroutine(OnExportSide());
            onExportPoint.Invoke();
        }

        if (!insideImport && !insideExport)
        {
            selfContainer.enabled = false;
            onTheWay.Invoke();
        }
    }

    private IEnumerator OnImportSide()
    {
        mover.Pause();

        while (!selfContainer.isFilledUp)
        {
            yield return new WaitForSeconds(1);
        }

        mover.Resume();
    }
    private IEnumerator OnExportSide()
    {
        while (true)
        {
            mover.Pause();

            while (!selfContainer.isEmpty)
            {
                yield return new WaitForSeconds(1);
            }

            mover.Resume();
            yield return new WaitForSeconds(1);
        }
    }

}
