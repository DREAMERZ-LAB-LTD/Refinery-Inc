using UnityEngine;
using IdleArcade.Core;
using Tutorial;
public class TrashBinSequence : TriggerableTutorial
{
    [Header("Detection Setup")]
    [SerializeField] private TransactionBridgeLimit playerLimit;
    [SerializeField] private string sourceTag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(sourceTag))
        { 
            if (playerLimit.isFilledUp)
            { 
                FireEvent(0);
                FireEvent(1);
            }
        }
    }

}
