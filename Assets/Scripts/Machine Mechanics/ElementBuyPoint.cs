using UnityEngine;
using IdleArcade.Core;
using System.Collections;
using General.Library;

[RequireComponent(typeof(TriggerDetector))]
public class ElementBuyPoint : MonoBehaviour, TriggerDetector.ITriggerable
{
    [SerializeReference] RawGarbageImporterNPC npc;

    private IEnumerator BuyRoutine()
    {
        while (Input.GetMouseButton(0))
            yield return null;

                npc.AddNewOrder();

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
