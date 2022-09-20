using SWS;
using UnityEngine;
using UnityEngine.Events;

public abstract class WayPointNPC : MonoBehaviour
{
    [Header("Movement Setup")]
    [SerializeField] protected splineMove mover;
    [SerializeField] protected int importPoint;
    [SerializeField] protected int exportPoint;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnImportSide;
    [SerializeField] private UnityEvent OnExportSide;
    [SerializeField] private UnityEvent m_OnTheWay;
    protected virtual void OnEnable()
    {
        mover.movementChangeEvent += OnChangePoint;
    }
    protected virtual void OnDisable()
    {
        mover.movementChangeEvent -= OnChangePoint;
    }


    private void OnChangePoint(int index)
    {
        var insideImport = importPoint == index;
        var insideExport = exportPoint == index;

        if (insideImport)
        { 
            OnImportSIde();
            OnImportSide.Invoke();
        }

        if (insideExport)
        { 
            OnExportSIde();
            OnExportSide.Invoke();
        }

        if (!insideImport && !insideExport)
        { 
            OnTheWay();
            m_OnTheWay.Invoke();
        }
    }

    protected abstract void OnImportSIde();

    protected abstract void OnExportSIde();

    protected abstract void OnTheWay();

}
