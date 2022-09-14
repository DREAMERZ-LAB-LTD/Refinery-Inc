using UnityEngine;
using IdleArcade.Core;
using SWS;

public class ClientCar : Containable
{
    public delegate void OnSide();
    public OnSide OnImportSide;
    public OnSide OnExportSide;

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
        { 
            OnImportSIde();
            if (OnImportSide != null)
                OnImportSide.Invoke();
        }

        if (insideExport)
        { 
            OnExportSIde();
            if (OnExportSide != null)
                OnExportSide.Invoke();
        }

        if (!insideImport && !insideExport)
            OnTheWay();
    }
}

