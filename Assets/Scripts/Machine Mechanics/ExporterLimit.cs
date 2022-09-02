using UnityEngine;
using IdleArcade.Core;

public class ExporterLimit : Limiter
{
    [SerializeField, Range(0, 100)] private float parcent = 50;
    [SerializeField] private UpgradeableLimiter importLimit;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        OnImporterUpgrade(0);
    }
#endif
    private void Awake()
    {
        OnImporterUpgrade(0);
        importLimit.OnUpgrade += OnImporterUpgrade;
    }

    private void OnDestroy()
    {
        importLimit.OnUpgrade -= OnImporterUpgrade;
    }
        
    private void OnImporterUpgrade(float t)
    {
        t = parcent / 100f;
        range.y = importLimit.GetCurrent * t;
    }
}
