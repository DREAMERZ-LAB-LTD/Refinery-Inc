using System.Collections;
using UnityEngine;
using General.Library;

public class GarbageImporter : NPC_CarBehaviour
{
    [SerializeField] private int unitPrice = 5;
    private int amount = 0;

    public void SetOrder(int deltaAmount)
    {
        if (ScoreManager.instance.AddScore(-Mathf.Abs(deltaAmount * unitPrice)))
            amount += deltaAmount;
    }

    protected override void OnImportSIde()
    {
        selfContainer.enabled = true;
        mover.Pause();
        StartCoroutine(BuyRoutine());
       
        IEnumerator BuyRoutine()
        {
            while (selfContainer.Getamount <= 0)
            {
                selfContainer.Add(amount);
                yield return new WaitForSeconds(1);
            }
            Debug.Log("Buy");

            mover.Resume();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            selfContainer.enabled = true;
    }
}
