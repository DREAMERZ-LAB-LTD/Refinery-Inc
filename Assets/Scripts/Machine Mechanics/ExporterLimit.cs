using UnityEngine;
using IdleArcade.Core;

public class ExporterLimit : Limiter
{
    [SerializeField, Range(0, 100)] private float parcent = 50;
    [SerializeField] private UpgradeableLimiter importLimit;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        OnImporterUpgrade(0, Vector2.right);
    }
#endif
    private void Awake()
    {
        OnImporterUpgrade(0, Vector2.right);
        importLimit.OnUpgrade += OnImporterUpgrade;
    }

    private void OnDestroy()
    {
        importLimit.OnUpgrade -= OnImporterUpgrade;
    }
        
    private void OnImporterUpgrade(float t, Vector2 range)
    {
        t = parcent / 100f;
        range.y = importLimit.GetCurrent * t;
    }
}
