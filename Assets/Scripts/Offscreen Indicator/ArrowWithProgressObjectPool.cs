using System.Collections.Generic;
using UnityEngine;

public class ArrowWithProgressObjectPool : MonoBehaviour
{
    public static ArrowWithProgressObjectPool current;

    [Tooltip("Assign the arrow prefab.")]
    public Indicator pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    List<Indicator> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<Indicator>();

        for (int i = 0; i < pooledAmount; i++)
        {
            Indicator arrowWithProgress = Instantiate(pooledObject);
            arrowWithProgress.transform.SetParent(transform, false);
            arrowWithProgress.Activate(false);
            pooledObjects.Add(arrowWithProgress);
        }
    }

    /// <summary>
    /// Gets pooled objects from the pool.
    /// </summary>
    /// <returns></returns>
    public Indicator GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            Indicator arrowWithProgress = Instantiate(pooledObject);
            arrowWithProgress.transform.SetParent(transform, false);
            arrowWithProgress.Activate(false);
            pooledObjects.Add(arrowWithProgress);
            return arrowWithProgress;
        }
        return null;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator arrow in pooledObjects)
        {
            arrow.Activate(false);
        }
    }
}
