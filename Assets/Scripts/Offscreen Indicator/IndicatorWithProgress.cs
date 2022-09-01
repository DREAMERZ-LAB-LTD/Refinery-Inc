using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorWithProgress : Indicator
{
    [SerializeField] private Image progress;

    public void SetProgressValue(float t)
    {
        if (progress == null) return;

        if (progress.type != Image.Type.Filled)
            progress.type = Image.Type.Filled;

        t = Mathf.Clamp01(t);
        progress.fillAmount = t;
    }

    public override void SetImageColor(Color color)
    {
        base.SetImageColor(color);

        if (progress)
            progress.color = color;
    }
}
