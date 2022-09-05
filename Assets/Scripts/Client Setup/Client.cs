using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleArcade.Core;

public class Client : MonoBehaviour
{
   
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

    private IEnumerator OrderGeneratorRoutine()
    {
        int totalCount = containers.Length;
        string[] validIDs = new string[totalCount];
        for (int i = 0; i < totalCount; i++)
            validIDs[i] = containers[i].GetID;

        List<string> tempIDs = null;
        List<string> args = new List<string>();
        int maxArgs = maxItemCount <= validIDs.Length ? maxItemCount: validIDs.Length; 
        while (true)
        {
            if (order == null)
            {
                yield return new WaitForSeconds(Random.Range(1f, 2f));

                tempIDs = new List<string>(validIDs);
                args.Clear();

                for (int i = 0; i < maxArgs; i++)
                {
                    args.Add(tempIDs[i]);
                    tempIDs.RemoveAt(i);
                }

                var newOrder = management.GenerateNewOrder(args);
                OnAddingOrder(newOrder);
            }
            else
                yield return new WaitForSeconds(5);
        }
    }


    private void OnRemoveOrder(Order order)
    {
        if (this.order == order)
            this.order = null;
    }

    private void OnAddingOrder(Order order)
    {
        if (order == null)
            return;

        order.OnCompleted += OnRemoveOrder;
        order.OnFailed += OnRemoveOrder;
        order.OnRejected += OnRemoveOrder;

        this.order = order;
    }
}
