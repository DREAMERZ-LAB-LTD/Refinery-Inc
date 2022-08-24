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
        fromContainer.OnFilled += OnChangeFroContainer;
    }

    private void OnDestroy()
    {
        fromContainer.OnFilled -= OnChangeFroContainer;
    }

    private void OnChangeFroContainer()
    {
        for (int i = 0; i < fromContainer.Getamount; i++)
            toContainer.Add(1);
    }
}
