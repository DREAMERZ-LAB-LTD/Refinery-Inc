using IdleArcade.Core;
using UnityEngine;

public abstract class NPCBehaviour : MonoBehaviour
{
    [Header("Container Setup")]
    [SerializeField] protected TransactionContainer[] containers;

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
            var isFilledUp = true;
            for (int i = 0; i < containers.Length; i++)
                if (containers[i].enabled)
                    isFilledUp &= containers[i].isFilledUp;

            if (isFilledUp)
                OnFilledUp();
            OnElementAdding();
        }

        if (delta < 0)
        {
            var isEmpty = true;
            for (int i = 0; i < containers.Length; i++)
                if (containers[i].enabled)
                    isEmpty &= containers[i].isEmpty;

            if (isEmpty)
                OnEmpty();

            OnElementRemoving();
        }
    }


    protected abstract void OnElementAdding();
    protected abstract void OnElementRemoving();
    protected abstract void OnEmpty();
    protected abstract void OnFilledUp();
}
