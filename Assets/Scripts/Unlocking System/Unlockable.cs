using UnityEngine;
using UnityEngine.Events;
using IdleArcade.Core;

[RequireComponent(typeof(TransactionContainer))]

public class Unlockable : MonoBehaviour
{
    public UnlockingData unlockingData;
    [SerializeField] protected UnityEvent OnUnlocked;

    private TransactionContainer container;
    private  void Awake()
    {
        if (unlockingData)
        {
            if (unlockingData.isUnlocked)
                OnUnlocked.Invoke();
            else
            {
                container = GetComponent<TransactionContainer>();
                if(container)
                    container.OnFilled += Unlock;
            }
        }
    }

    protected void OnDestroy()
    {
        if (container)
            container.OnFilled -= Unlock;
    }
    private void Unlock()
    {
        unlockingData.isUnlocked = true;
        OnUnlocked.Invoke();
    }
}
