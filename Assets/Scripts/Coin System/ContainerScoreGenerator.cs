using UnityEngine;
using General.Library;
using IdleArcade.Core;

public class ContainerScoreGenerator : ScoreGenerator
{
    [Header("Container Setup")]
    [SerializeField] TransactionContainer toContainer = null;
    [SerializeField] private TransactionContainer fromContainer;
    private void Awake()
    {
        fromContainer.OnChangedValue += OnChangeFroContainer;
    }

    private void OnDestroy()
    {
        fromContainer.OnChangedValue -= OnChangeFroContainer;
    }

    private void OnChangeFroContainer(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        Debug.Log("Changing  " + transform.parent.name + " Delata = " + delta);
        if (delta <= 0) return;
        toContainer.Add(delta);
        GenerateScore();

    }
}
