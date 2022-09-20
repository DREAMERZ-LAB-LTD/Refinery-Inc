using General.Library;
using IdleArcade.Core;
using System.Collections;
using UnityEngine;

public class RawGarbageImporterNPC : WayPointNPC
{
    [Header("Container Setup")]
    [SerializeField] protected TransactionContainer selfContainer;
    [SerializeField] protected TransactionContainer sourceContainer;

    [Header("Pricing Setup")]
    [SerializeField] private int buyAmount = 20;
    [SerializeField] private int unitPrice = 5;
    
    [Header("Hydraulic Setup")]
    [SerializeReference] private Transform carrierHydraulic;
    [SerializeField] private float bumpDuration = 1;
    [SerializeField] private Vector3 rotationA;
    [SerializeField] private Vector3 rotationB;


    [SerializeField] private int orderAmount = 0;

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

  

    public void AddNewOrder()
    {
        if (sourceContainer.Getamount <= 0)
            return;

        var dt = sourceContainer.Getamount % buyAmount + 1;
        Debug.Log("DT = " + dt);
        if (!ScoreManager.instance.AddScore(-Mathf.Abs(dt * unitPrice)))
            return;

        orderAmount += dt;
        selfContainer.amountLimit.range.y = orderAmount;
    }

    protected override void OnTheWay()
    {
        StopAllCoroutines();
    }

    protected override void OnImportSIde()
    {
        StopAllCoroutines();
        StartCoroutine(OnImportSide());

        IEnumerator OnImportSide()
        {

            int preAmount = -1;
            mover.Pause();
            selfContainer.enabled = true;

            while (selfContainer.isEmpty || orderAmount == 0)
                yield return new WaitForSeconds(1);

            while (preAmount != selfContainer.Getamount)
            {
                preAmount = selfContainer.Getamount;
                yield return new WaitForSeconds(3);
            }

            orderAmount -= selfContainer.Getamount;
            selfContainer.enabled = false;
            mover.Resume();
        }
    }

    protected override void OnExportSIde()
    {
        StopAllCoroutines();
        if(!selfContainer.isEmpty)
            StartCoroutine(OnExportSide());

        IEnumerator OnExportSide()
        {
            mover.Pause();

            float startTime = Time.time;
            float endTime = startTime + bumpDuration;
            float t;
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                carrierHydraulic.localRotation = Quaternion.Euler(Vector3.Lerp(rotationA, rotationB, t));
                yield return null;
            }


            selfContainer.enabled = true;
            while (!selfContainer.isEmpty)
                yield return new WaitForSeconds(1);
            selfContainer.enabled = false;

            startTime = Time.time;
            endTime = startTime + bumpDuration;
            while (Time.time < endTime)
            {
                t = Mathf.InverseLerp(startTime, endTime, Time.time);
                carrierHydraulic.localRotation = Quaternion.Euler(Vector3.Lerp(rotationB, rotationA, t));
                yield return null;
            }

            mover.Resume();

        }
    }
}
