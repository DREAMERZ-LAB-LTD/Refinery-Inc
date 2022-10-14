using UnityEngine;
using IdleArcade.Core;
using System.Collections;
using General.Library;
using UnityEngine.Events;

[RequireComponent(typeof(TriggerDetector))]
public class ElementBuyPoint : MonoBehaviour, TriggerDetector.ITriggerable
{
    [SerializeReference] RawGarbageImporterNPC npc;
    [Header("Pricing Setup")]
    [SerializeField] private int buyAmount = 20;
    [SerializeField] private int unitPrice = 1;

    [SerializeField] private UnityEvent OnPurchase;

    private IEnumerator BuyRoutine()
    {
        while (Input.GetMouseButton(0))
            yield return null;

        var available = npc.sourceContainer.Getamount;
        var pendingAmount = npc.GetPendingQuantity();
        var newTarget = pendingAmount + buyAmount;
        var dt = buyAmount;

        if (newTarget > available)
            dt = available;

        if (GameManager.instance.coinContainer.Add(-Mathf.Abs(dt * unitPrice)))
        { 
            npc.AddNewOrder(dt);
            OnPurchase.Invoke();
        }

    }

    public void OnEnter(Collider other)
    {
        StartCoroutine(BuyRoutine());
    }

    public void OnExit(Collider other)
    {
        StopAllCoroutines();
    }
}
