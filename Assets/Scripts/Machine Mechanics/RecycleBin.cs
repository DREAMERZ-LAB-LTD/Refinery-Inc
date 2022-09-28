using UnityEngine;
using IdleArcade.Core;
using General.Library;
using System.Collections;

public class RecycleBin : MonoBehaviour, TriggerDetector.ITriggerable
{
    [System.Serializable]
    private class MaterialSet
    {
        public string id;
        public int price;
        public int garbageUnit;
    }

    [Header("Visual Flow Setup")]
    [SerializeField] private Transform target;
    [SerializeField] private float visualFlowDuration = .5f;

    [Header("Recycle Bin Setup")]
    [SerializeField] private float timeInterval = 0.5f;
    [SerializeField] TransactionContainer garbageSource;
    [SerializeField] private MaterialSet[] recycleSets;


    private Coroutine recycleRoutine = null;

    public void OnEnter(Collider other)
    {
        var containable = other.GetComponent<Containable>();
        var visual = other.GetComponent<TransactionVisualCore>();
        var bridgeLimit = containable.GetComponent<TransactionBridgeLimit>();
        if (containable == null || visual == null) return;

        if (recycleRoutine != null)
            StopCoroutine(recycleRoutine);
        recycleRoutine = StartCoroutine(RecycleRoutine(visual, containable, bridgeLimit));
    }

    public void OnExit(Collider other)
    {
        if (recycleRoutine != null)
            StopCoroutine(recycleRoutine);
    }

    private MaterialSet GetMaterialSet(in string id)
    {
        for (int i = 0; i < recycleSets.Length; i++)
            if (recycleSets[i].id == id)
                return recycleSets[i];

        return null;
    }

    private IEnumerator RecycleRoutine(TransactionVisualCore visual, Containable containable, TransactionBridgeLimit bridgeLimit)
    {
        var targetID = visual.GetNextID_UsingFIFO();
        var sourceContainer = containable.GetContainer(targetID);
        var materialSet = GetMaterialSet(targetID);

        if (materialSet == null)
            yield break;

        while (sourceContainer)
        {
            while (!sourceContainer.isEmpty)
            {
                garbageSource.Add(materialSet.garbageUnit);
                sourceContainer.Add(-1);
                if (bridgeLimit)
                    bridgeLimit.Transact(-1);

                ScoreManager.instance.AddScore(materialSet.price);

                var entity = visual.Pull_UsingFIFO(targetID);
                entity.transform.parent = transform;
                StartCoroutine(MoveTo(entity, target.position, visualFlowDuration));


                if (timeInterval > 0)
                    yield return new WaitForSeconds(timeInterval);
                else
                    yield return null;
            }

            targetID = visual.GetNextID_UsingFIFO();
            sourceContainer = containable.GetContainer(targetID);
            materialSet = GetMaterialSet(targetID);

            if (materialSet == null)
                yield break;
        }
    }

    private IEnumerator MoveTo(Entity entity, Vector3 target, float duration)
    {
        float t;
        var startTime = Time.time; ;
        var endTime = startTime + duration;
        Vector3 startPoint = entity.transform.position;
        while (endTime > Time.time)
        {
            t = Mathf.InverseLerp(startTime, endTime, Time.time);
            entity.transform.position = Vector3.Lerp(startPoint, target, t);
            yield return null;
        }

        Destroy(entity.gameObject);
    }
}
