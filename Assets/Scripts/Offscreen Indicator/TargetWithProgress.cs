using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithProgress : Target
{
    [Range(0, 1)]
    public float progress = 0.5f;
}
