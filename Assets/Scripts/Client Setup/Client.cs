using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleArcade.Core;

public class Client : MonoBehaviour
{
    [SerializeField] private OrderStatusUI carOrderStatus;
    public OrderManagement management;
    [SerializeField] private int maxItemCount = 1;

    public Order order;
    [SerializeField] private TransactionContainer[] containers;

    private void Awake()
    {
        StartCoroutine(OrderGeneratorRoutine());
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnRemoveOrder(Order order)
    {
        order.OnCompleted -= OnRemoveOrder;
        order.OnFailed -= OnRemoveOrder;
        order.OnRejected -= OnRemoveOrder;
        this.order = null;
    }

    private void OnAddingOrder(Order order)
    {
        if (order == null)
            return;

        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].enabled = false;
            for (int j = 0; j < order.items.Count; j++)
            {
                if (containers[i].GetID == order.items[j].iD)
                {
                    containers[i].enabled = true;
                    break;
                }
            }
        }


        order.OnCompleted += OnRemoveOrder;
        order.OnFailed += OnRemoveOrder;
        order.OnRejected += OnRemoveOrder;
        order.OnAccepted += carOrderStatus.ShowOrder;
        this.order = order;
    }


    private IEnumerator OrderGeneratorRoutine()
    {
        int totalCount = containers.Length;
        string[] validIDs = new string[totalCount];
        for (int i = 0; i < totalCount; i++)
            validIDs[i] = containers[i].GetID;

        List<string> tempIDs = null;
        List<string> args = new List<string>();
        int maxArgs = maxItemCount <= validIDs.Length ? maxItemCount : validIDs.Length;

        while (true)
        {
            if (order == null)
            {
                yield return new WaitForSeconds(Random.Range(3f, 5f));

                tempIDs = new List<string>(validIDs);
                args.Clear();

                for (int i = 0; i < maxArgs; i++)
                {
                    int inxex = Random.Range(0, tempIDs.Count);
                    args.Add(tempIDs[inxex]);
                    tempIDs.RemoveAt(inxex);
                }

                var newOrder = management.GenerateNewOrder(args);
                OnAddingOrder(newOrder);
            }
            else
                yield return new WaitForSeconds(10);
        }
    }

}
