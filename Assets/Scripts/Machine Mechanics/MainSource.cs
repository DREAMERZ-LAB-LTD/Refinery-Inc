using UnityEngine;
using IdleArcade.Core;
using System.Collections;

[RequireComponent(typeof(TriggerDetector))]
public class MainSource : MonoBehaviour, TriggerDetector.ITriggerable
{
 
    [SerializeField] private int deltaAmount = 20;

    private IEnumerator BuyRoutine()
    {
        while (Input.GetMouseButton(0))
            yield return null;

  //      importer.SetOrder(deltaAmount);
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
