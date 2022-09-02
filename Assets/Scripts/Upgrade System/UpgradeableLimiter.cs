using UnityEngine;
using IdleArcade.Core;
using UnityEngine.Events;

public class UpgradeableLimiter : Limiter
{
    public delegate void Upgrade(float t);
    public Upgrade OnUpgrade;
    [SerializeField] UnityEvent OnLocked;
    [SerializeField] UnityEvent OnUnlocked;

    protected virtual void Start()
    {
        var data = UpgradeSystem.instance.GetDataField(GetID);
        if (data != null)
        {
            t = data.T;
            On_Unlocking(data.isUnlocked);
            data.OnChanged += On_Upgrade;
            data.OnUnlocking += On_Unlocking;
        }
    }

    protected virtual void OnDestroy()
    {
        if (UpgradeSystem.instance == null) return;

        var data = UpgradeSystem.instance.GetDataField(GetID);
        if (data != null)
        {
            t = data.T;
            data.OnChanged -= On_Upgrade;
            data.OnUnlocking -= On_Unlocking;
        }
    }

    private void On_Unlocking(bool isUnlock)
    {
        if (isUnlock)
            OnUnlocked.Invoke();
        else
            OnLocked.Invoke();
    }
    protected virtual void On_Upgrade(float t)
    {
        this.t = t;
        if (OnUpgrade != null)
            OnUpgrade.Invoke(t);
    }
}
