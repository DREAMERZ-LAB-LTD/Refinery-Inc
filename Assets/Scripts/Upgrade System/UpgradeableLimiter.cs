using UnityEngine;
using IdleArcade.Core;
using UnityEngine.Events;

public class UpgradeableLimiter : Limiter
{
    [SerializeField] UnityEvent OnLocked;
    [SerializeField] UnityEvent OnUnlocked;

    protected virtual void Start()
    {
        var data = UpgradeSystem.instance.GetDataField(GetID);
        if (data != null)
        {
            t = data.T;
            OnUnlocking(data.isUnlocked);
            data.OnChanged += OnUpgrade;
            data.OnUnlocking += OnUnlocking;
        }
    }

    protected virtual void OnDestroy()
    {
        if (UpgradeSystem.instance == null) return;

        var data = UpgradeSystem.instance.GetDataField(GetID);
        if (data != null)
        {
            t = data.T;
            data.OnChanged -= OnUpgrade;
            data.OnUnlocking -= OnUnlocking;
        }
    }

    private void OnUnlocking(bool isUnlock)
    {
        if (isUnlock)
            OnUnlocked.Invoke();
        else
            OnLocked.Invoke();
    }
    private void OnUpgrade(float t)
    {
        this.t = t;
    }
}
