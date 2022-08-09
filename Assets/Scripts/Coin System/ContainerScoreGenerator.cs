using UnityEngine;
using General.Library;
using IdleArcade.Core;

public class ContainerScoreGenerator : ScoreGenerator
{
    [SerializeField] bool OnAdding = true;
    [SerializeField] bool OnRemoving = false;
    [SerializeField] private TransactionContainer[] containers;
    private void Awake()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].OnChangedValue += OnScoring;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].OnChangedValue -= OnScoring;
        }
    }

    private void OnScoring(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if (delta > 0 && OnAdding)
            GenerateScore();

        if (delta < 0 && OnRemoving)
            GenerateScore();
    }
}
