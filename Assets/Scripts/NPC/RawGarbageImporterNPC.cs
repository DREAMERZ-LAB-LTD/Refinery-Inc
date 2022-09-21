using General.Library;
using IdleArcade.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RawGarbageImporterNPC : WayPointNPC
{
    [Header("Container Setup")]
    [SerializeField] protected TransactionContainer selfContainer;
    [SerializeField] public TransactionContainer sourceContainer;

    [Header("Hydraulic Setup")]
    [SerializeReference] private Transform carrierHydraulic;
    [SerializeField] private float bumpDuration = 1;
    [SerializeField] private Vector3 rotationA;
    [SerializeField] private Vector3 rotationB;

    private List<int> orders = new List<int>();

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    public int GetPendingQuantity()
    {
        int pendingQuantity = 0;
        for (int i = 0; i < orders.Count; i++)
            pendingQuantity += orders[i];

        return pendingQuantity;
    }

    public void AddNewOrder(int quantity)
    {
        orders.Add(quantity);
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
            mover.Pause();
            while (orders.Count == 0)
                yield return new WaitForSeconds(1);

            var orderItemCount = orders[0];
            orders.RemoveAt(0);
            selfContainer.amountLimit.range.y = orderItemCount;

            selfContainer.enabled = true;

            while (selfContainer.isEmpty)
                yield return new WaitForSeconds(1);

            int preAmount = -1;
            while (preAmount != selfContainer.Getamount)
            {
                preAmount = selfContainer.Getamount;
                yield return new WaitForSeconds(1.3f);
            }

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
