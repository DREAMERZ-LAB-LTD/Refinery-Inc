using UnityEngine;
using IdleArcade.Core;
using UnityEngine.Events;

public class UnlockableContainer : TransactionContainer
{
    [Header("Unlocking Setup")]
    [SerializeField] private int targetLevel = 1;

    [Header("Unlocking Events")]
    [SerializeField] private UnityEvent OnEnoughLevel;
    [SerializeField] private UnityEvent OnLowLevel;

    public bool isValidToUnlock()
    {
        var currentLevel = GameManager.instance.playerExprence.Level;
        if (currentLevel >= targetLevel)
        {
            OnEnoughLevel.Invoke();
            return true;
        }
        else
        {
            OnLowLevel.Invoke();
            return false;
        }
    }
}
