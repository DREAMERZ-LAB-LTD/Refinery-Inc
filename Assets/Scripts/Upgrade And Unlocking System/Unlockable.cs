using UnityEngine;
using UnityEngine.Events;
using IdleArcade.Core;

[RequireComponent(typeof(TransactionContainer))]

public class Unlockable : Entity
{
    [SerializeField] protected UnityEvent OnLocked;
    [SerializeField] protected UnityEvent OnUnlocked;

    private TransactionContainer container;
    private  void Start()
    {
        var data = UpgradeSystem.instance.GetDataField(GetID);
        var isUnlcok = data.isUnlocked;
        UnlockStatus(isUnlcok);
        
        if(!isUnlcok)
        {
            container = GetComponent<TransactionContainer>();
            if (container)
            {
                container.amountLimit.range.y = data.unlockPrice;
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
        var data = UpgradeSystem.instance.GetDataField(GetID);

        data.isUnlocked = true;
        UnlockStatus(true);
    }

    private void UnlockStatus(bool isUnlock)
    {
        if (isUnlock)
            OnUnlocked.Invoke();
        else
           OnLocked.Invoke();
        
    }
}
