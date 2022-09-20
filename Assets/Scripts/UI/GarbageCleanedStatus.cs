using UnityEngine;
using IdleArcade.Core;
using UnityEngine.UI;

public class GarbageCleanedStatus : MonoBehaviour
{
    [SerializeReference] private TransactionContainer container;
    [SerializeReference] private Image statusBar;

    private void Awake()
    {
        statusBar.type = Image.Type.Filled;
        container.OnChangedValue += OnGarbageUpdate;
    }

    private void OnDestroy()
    {
        container.OnChangedValue -= OnGarbageUpdate;
    }
        

    private void OnGarbageUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        statusBar.fillAmount = 1 - currnet / (float)max;
    }
}
