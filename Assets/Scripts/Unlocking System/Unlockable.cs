using UnityEngine;
using UnityEngine.Events;

public class Unlockable : MonoBehaviour
{
    [SerializeField] private UnlockingData data;
    [SerializeField, Tooltip("Invoked when object is already unlocked and try to set initial state on game start")] 
    protected UnityEvent OnReinitialized;
    [SerializeField, Tooltip("Invoked when object will begin to unlock")] 
    protected UnityEvent OnUnlocked;

    protected virtual void Awake()
    {
        if (data)
            if (data.IsUnlocked)
            {
                OnReinitialized.Invoke();
                Destroy(this);
            }
    }

    public virtual void Unlock()
    {
        if (data)
            if (data.Unlock())
               OnUnlocked.Invoke();
    }
}
