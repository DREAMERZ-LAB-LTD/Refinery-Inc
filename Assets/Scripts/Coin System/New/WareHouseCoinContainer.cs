using General.Library;
using IdleArcade.Core;
using UnityEngine;

public class WareHouseCoinContainer : TransactionContainer
{
    private  void Start()
    {
        OnChangedValue += OnContainerUpdate;
    }

    private  void OnDestroy()
    {
        OnChangedValue -= OnContainerUpdate;
    }
    
    private void OnContainerUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    { 
        if(delta < 0)
            ScoreManager.instance.AddScore(Mathf.Abs(delta));
    }
}
