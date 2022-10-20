using UnityEngine;
using UnityEngine.Events;

public class TRS_Controller : MonoBehaviour
{
    [System.Flags]
    public enum TRS_Mode
    { 
        Transforming = 0x00000001,
        Rotating     = 0x00000002,
        Scaling      = 0x00000004
    }
    public enum LoopType
    { 
        None,
        Both,
        Clockwise,
        AntiClockwise
    }

    [SerializeField] private float speed = 0.3f;
    [SerializeField] private TRS_Mode tRS_Mode = TRS_Mode.Rotating;
    [SerializeField] private LoopType loopType = LoopType.Both;
    [SerializeField] private Transform a, b;
    private float t = 0;
    private bool finishedNoneLoop = false;

    [Header("callback Events")]
    [SerializeField, Tooltip("Event will be fire when  TRS complete each cycle ")] 
    private UnityEvent OnCycleComplete;


    public void SetPointA(Transform a) { this.a = a; }
    public void SetPointB(Transform b) { this.b = b; }
    public void Resetart()
    {
        this.t = 0;
        finishedNoneLoop = false;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        t += speed * Time.deltaTime;
        if(isValidLoop())
            ApplyTRS(t);

        t = Mathf.Clamp(t, 0, 1);
    }

    private bool isValidLoop()
    {
        switch (loopType)
        {
            case LoopType.None:
                if (t > 1 || t < 0)
                {
                    if (!finishedNoneLoop)
                    { 
                        OnCycleComplete.Invoke();
                        finishedNoneLoop = true;
                    }
                    return false;
                }
                break;
            case LoopType.Both:
                if (t >= 1 || t <= 0)
                {
                    speed = -speed;
                    OnCycleComplete.Invoke();
                }
                break;
            case LoopType.Clockwise:
                speed = Mathf.Abs(speed);
                if (t >= 1)
                {
                    t = 0;
                    OnCycleComplete.Invoke();
                }
                break;
            case LoopType.AntiClockwise:
                speed = -Mathf.Abs(speed);
                if (t <= 0)
                {
                    t = 1;
                    OnCycleComplete.Invoke();
                }
                break;
        }
        return true;
    }

    private void ApplyTRS(float t)
    {
        if ((tRS_Mode & TRS_Mode.Rotating) == TRS_Mode.Rotating)
            transform.localEulerAngles = Vector3.Lerp(a.localEulerAngles, b.localEulerAngles, t);

        if ((tRS_Mode & TRS_Mode.Transforming) == TRS_Mode.Transforming)
            transform.localPosition = Vector3.Lerp(a.localPosition, b.localPosition, t);
        
        if ((tRS_Mode & TRS_Mode.Scaling) == TRS_Mode.Scaling)
        { 
            t = Mathf.Sin(Mathf.Pow(t * Mathf.PI / 2, 2)) * Mathf.PI / 2;
            transform.localScale = Vector3.LerpUnclamped(a.localScale, b.localScale, t);
        }
    }
    
}
