using UnityEngine;

public class TargetWithProgress : Target
{
    [Range(0, 1)]
    public float progress = 0.5f;
    public void SetProgress(float t)
    {
        progress = Mathf.Clamp01(t);
    }
    public void SetProgress(float current, float max)
    {
        progress = Mathf.Clamp01(current / max);
    }
}
