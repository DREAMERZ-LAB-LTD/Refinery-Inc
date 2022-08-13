using UnityEngine;
using IdleArcade.Core;
using General.Library;

[RequireComponent(typeof(TriggerDetector))]
public class MainSource : MonoBehaviour, TriggerDetector.ITriggerable
{
    [SerializeField] private TransactionContainer container;
    [SerializeField] private int initialAmount = 15;
    [SerializeField] private int price = 10;
    [SerializeField] private string coinID;

    private void Start()
    {
        container.Add(initialAmount);
    }

    public void Buy()
    {
        if (container.willCrossLimit(initialAmount)) return;

        if (ScoreManager.instance.AddScore(-Mathf.Abs(price), coinID))
        {
            container.Add(initialAmount);
        }
    }

    public void OnEnter(Collider other)
    {
        Buy();
    }

    public void OnExit(Collider other)
    {
       
    }
}
