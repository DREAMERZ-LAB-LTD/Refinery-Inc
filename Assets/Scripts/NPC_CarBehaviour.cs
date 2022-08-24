using System.Collections;
using UnityEngine;
using SWS;
using IdleArcade.Core;
using UnityEngine.Events;

public class NPC_CarBehaviour : MonoBehaviour
{
    [Header("Container Setup")]
    [SerializeField] protected TransactionContainer selfContainer;
    [SerializeField] protected TransactionContainer sourceContainer;
    [Header("Movement Setup")]
    [SerializeField] protected splineMove mover;
    [SerializeField] protected int importPoint;
    [SerializeField] protected int exportPoint;


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
        StopAllCoroutines();

        var insideImport = importPoint == index;
        var insideExport = exportPoint == index;

        if (insideImport)
            OnImportSIde();
    
        if (insideExport)
            OnExportSIde();
        
        if (!insideImport && !insideExport)
            OnTheWay();
        
    }

    protected virtual void OnImportSIde()
    {
        selfContainer.enabled = true;
        StartCoroutine(OnImportSide());
    }

    protected virtual void OnExportSIde()
    {
        selfContainer.enabled = true;
        StartCoroutine(OnExportSide());
    }

    protected virtual void OnTheWay()
    { 
        selfContainer.enabled = false;
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
                yield return new WaitForSeconds(1);

            if (sourceContainer)
            { 
                while(sourceContainer.Getamount == 0)
                    yield return new WaitForSeconds(1);
            }

            mover.Resume();
            yield return new WaitForSeconds(1);
        }
    }
}
