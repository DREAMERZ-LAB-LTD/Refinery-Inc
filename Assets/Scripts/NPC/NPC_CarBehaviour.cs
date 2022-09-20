using System.Collections;
using UnityEngine;
using IdleArcade.Core;

public class NPC_CarBehaviour : WayPointNPC
{
    [Header("Container Setup")]
    [SerializeField] protected TransactionContainer selfContainer;
    [SerializeField] protected TransactionContainer sourceContainer;

    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

 
    protected override void OnImportSIde()
    {
        StopAllCoroutines();
        selfContainer.enabled = true;
        StartCoroutine(OnImportSide());
    }

    protected override void OnExportSIde()
    {
        StopAllCoroutines();
        selfContainer.enabled = true;
        StartCoroutine(OnExportSide());
    }

    protected override void OnTheWay()
    {
        StopAllCoroutines();
        selfContainer.enabled = false;
    }

    private IEnumerator OnImportSide()
    {
        mover.Pause();

        while (selfContainer.isEmpty)
            yield return new WaitForSeconds(1);

        int preAmount = -1;
        while (preAmount != selfContainer.Getamount)
        {
            preAmount = selfContainer.Getamount;
            yield return new WaitForSeconds(1.3f);
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
