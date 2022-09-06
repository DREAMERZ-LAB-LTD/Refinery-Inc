using UnityEngine;
using IdleArcade.Core;
using SWS;
using System.Collections;

public class ClientCar : Containable
{
    [Header("Movement Setup")]
    [SerializeField] protected splineMove mover;
    [SerializeField] protected int importPoint;
    [SerializeField] protected int exportPoint;
    public TransactionVisualCore visual;

    private void OnEnable()
    {
        mover.movementChangeEvent += OnChangePoint;
    }
    private void OnDisable()
    {
        mover.movementChangeEvent -= OnChangePoint;
        StopAllCoroutines();
    }

    protected virtual void OnImportSIde()
    {
        mover.Pause();
    }
    protected virtual void OnTheWay() { }

    protected virtual void OnExportSIde()
    {
        mover.Pause();
        for (int i = 0; i < containers.Length; i++)
            containers[i].TransactFrom(-containers[i].Getamount, containers[i]);
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
}

