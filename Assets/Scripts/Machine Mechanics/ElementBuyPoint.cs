using UnityEngine;
using IdleArcade.Core;
using System.Collections;
using General.Library;

[RequireComponent(typeof(TriggerDetector))]
public class ElementBuyPoint : MonoBehaviour, TriggerDetector.ITriggerable
{
    [SerializeField] private int buyAmount = 20;
    [SerializeField] private int unitPrice = 5;

    public int orderAmount = 0;
    private IEnumerator BuyRoutine()
    {
        while (Input.GetMouseButton(0))
            yield return null;

        if (ScoreManager.instance.AddScore(-Mathf.Abs(buyAmount * unitPrice)))
            orderAmount += buyAmount;
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
