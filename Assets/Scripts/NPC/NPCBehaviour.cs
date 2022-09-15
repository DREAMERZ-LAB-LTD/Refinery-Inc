using IdleArcade.Core;
using UnityEngine;

public abstract class NPCBehaviour : Containable
{
    protected virtual void Start()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue += OnChangedValue;
        
    }

    protected virtual void OnDestroy()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue -= OnChangedValue;
        
    }

    private void OnChangedValue(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if (delta > 0)
        { 
            OnElementAdding(containerID, currnet, max);

            var isFilledUp = true;
            for (int i = 0; i < containers.Length; i++)
                if (containers[i].enabled)
                    isFilledUp &= containers[i].isFilledUp;

            if (isFilledUp)
                OnFilledUp();
        }
        if (delta < 0)
        { 
            OnElementRemoving(containerID, currnet, max);

            var isEmpty = true;
            for (int i = 0; i < containers.Length; i++)
                if (containers[i].enabled)
                    isEmpty &= containers[i].isEmpty;

            if (isEmpty)
                OnEmpty();
        }
    }


    protected abstract void OnElementAdding(string id, int currnet, int max);
    protected abstract void OnElementRemoving(string id, int currnet, int max);
    protected abstract void OnEmpty();
    protected abstract void OnFilledUp();
}
