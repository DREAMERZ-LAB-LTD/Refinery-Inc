using UnityEngine;
using IdleArcade.Core;
using General.Library;
using System.Collections;

[RequireComponent(typeof(TriggerDetector))]
public class MainSource : MonoBehaviour, TriggerDetector.ITriggerable
{
    [SerializeField] private TransactionContainer container;
    [SerializeField] private int initialAmount = 100;
    [SerializeField] private int deltaAmount = 50;
    [SerializeField] private int price = 10;
    [SerializeField] private string coinID;

    private int GetLastAmount 
    {
        set { PlayerPrefs.SetInt("Progress", value); }
        get
        {
            if (PlayerPrefs.HasKey("Progress"))
                return PlayerPrefs.GetInt("Progress");
            else
                return initialAmount;
        }
    }

    private void Start()
    {
        container.OnChangedValue += OnContainerUpdate;
        container.Add(GetLastAmount);
    }

    private void OnDestroy()
    {
        container.OnChangedValue -= OnContainerUpdate;
    }
        
    private void OnContainerUpdate(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        GetLastAmount = currnet;
    }

    public void Buy()
    {
        if (container.willCrossLimit(deltaAmount)) return;

        if (ScoreManager.instance.AddScore(-Mathf.Abs(price)))
        {
            container.Add(deltaAmount);
        }
    }

    private IEnumerator BuyRoutine()
    {
        while (Input.GetMouseButton(0))
            yield return null;

        Buy();
    }

    public void OnEnter(Collider other)
    {
        StartCoroutine(BuyRoutine());
    }

    public void OnExit(Collider other)
    {
        StopAllCoroutines();
    }
}
