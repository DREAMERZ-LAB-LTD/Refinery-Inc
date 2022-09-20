using UnityEngine;
using IdleArcade.Core;
using General.Library;

public class RawGarbageContainer : TransactionContainer
{
    [SerializeField] private int unitPrice;
    public void Buy(int amount)
    {
        if (ScoreManager.instance.AddScore(-Mathf.Abs(amount * unitPrice)))
        { 

        }
    }


}
