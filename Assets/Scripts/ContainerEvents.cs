using UnityEngine;
using IdleArcade.Core;
using UnityEngine.Events;

public class ContainerEvents : MonoBehaviour
{
    [SerializeField, Tooltip("Target Container Object")]
    protected TransactionContainer container;

    [SerializeField] protected UnityEvent OnAdding;
    [SerializeField] protected UnityEvent OnRemoving;
    [SerializeField] protected UnityEvent OnFilledUp;
    [SerializeField] protected UnityEvent OnEmpty;

    protected virtual void Awake()
    {
        container.OnEmpty += OnEmpty.Invoke;
        container.OnFilled += OnFilledUp.Invoke;
        container.OnChangedValue += OnChangeValue;
    }

    protected virtual void OnDestroy()
    {
        container.OnEmpty -= OnEmpty.Invoke;
        container.OnFilled -= OnFilledUp.Invoke;
        container.OnChangedValue -= OnChangeValue;
    }
    protected virtual void OnChangeValue(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if (delta > 0)
            OnAdding.Invoke();
        if (delta < 0)
            OnRemoving.Invoke();
    }
}
