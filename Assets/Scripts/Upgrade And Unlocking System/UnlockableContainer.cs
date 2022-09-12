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
        var currentLevel = (int)GameManager.instance.playerExprence.GetCurrent;
        if (currentLevel >= targetLevel)
        {
            Debug.Log("Valid Level to Unlock" + targetLevel);
            OnEnoughLevel.Invoke();
            return true;
        }
        else
        {
            Debug.Log("InValid Level to Unlock" + targetLevel);
            OnLowLevel.Invoke();
            return false;
        }
    }
}
