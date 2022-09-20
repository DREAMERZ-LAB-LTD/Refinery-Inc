using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleArcade.Core;

public class GarbageStackVisualizer : TransactionVisualCore
{
    [SerializeReference] private Transform point;
    [SerializeField] private Vector3 minPoint;
    [SerializeField] private Vector3 maxPoint;
    protected override void OnAdding(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        var t =  currnet /(float)max;
        point.localPosition = Vector3.Lerp(minPoint, maxPoint, t);
    }

    protected override void OnRemoving(int delta, int currnet, int max, TransactionContainer A, TransactionContainer B)
    {
        var t = currnet / (float)max;
        point.localPosition = Vector3.Lerp(minPoint, maxPoint, t);
    }
}
