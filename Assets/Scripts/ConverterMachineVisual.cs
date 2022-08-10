using System.Collections;
using UnityEngine;
using IdleArcade.Core;

public class ConverterMachineVisual : TransactionSourceVisual
{
    [SerializeField] private string timeIntervalLimitID;
    protected Limiter timeintervallimit;

    public Transform spwnPoint;

    protected override void Awake()
    {
        base.Awake();
    
        var limits = GetComponents<Limiter>();
        foreach (var limit in limits)
            if (limit.GetID == timeIntervalLimitID)
            {
                timeintervallimit = limit;
                break;
            }
    }

    protected override void OnAdding(int delta, TransactionContainer A)
    {
        var entity = SpawnEntity(spwnPoint.position, spwnPoint.rotation, transform);
        float delay = 0f;
        if (timeintervallimit)
            delay = timeintervallimit.GetCurrent;
        StartCoroutine(Move(entity, spwnPoint.position, transform.position, delay));
    }


    private IEnumerator Move(Entity entity, Vector3 from, Vector3 to, float duration )
    {
        float startTime = Time.time;
        float endTIme = startTime + duration;
        float t = 0;

        while (Time.time < endTIme)
        {
            t = Mathf.InverseLerp(startTime, endTIme, Time.time);
            entity.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        Push(entity);
    }
}
