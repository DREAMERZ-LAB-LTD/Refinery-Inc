using UnityEngine;
using General.Library; 
using IdleArcade.Core;
public class CoinGenerator : MonoBehaviour
{
    [SerializeField] private string coinID;
    [SerializeField] private int amount;
    private TransactionContainer container;
    private void Awake()
    {
        container = GetComponent<TransactionContainer>();
        container.OnChangedValue += OnContainerUpdate;
    }

    private void OnDestroy()
    {
        if(container)
            container.OnChangedValue -= OnContainerUpdate;
    }

    private void OnContainerUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if (delta < 0)
            ScoreManager.instance.AddScore(amount, coinID);
    }

}
