using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IdleArcade.Core;

public class Client : MonoBehaviour
{
    [SerializeField] private int maxItemCount = 1;
    [SerializeField] private TargetWithProgress arrowProgress;
    [SerializeField] private OrderStatusUI carOrderStatus;

    public Order order;
    [SerializeField] private TransactionContainer[] containers;

    private void Awake()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue += OnTransact;

        StartCoroutine(OrderGeneratorRoutine());
    }
    private void OnDestroy()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue -= OnTransact;
        StopAllCoroutines();
    }

    private void OnTransact(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if (order != null)
            order.FillUpItem(B.GetID, delta);
    }


    private void OnRemoveOrder(Order order)
    {
        order.OnCompleted -= OnRemoveOrder;
        order.OnFailed -= OnRemoveOrder;
        order.OnRejected -= OnRemoveOrder;
        
        if (order.isAccepted)
        { 
            order.OnAccepted -= OnOrderAccepted;
            if (arrowProgress)
            { 
                order.OnChangeDeliveryTime -= arrowProgress.SetProgress;
                arrowProgress.enabled = false;
            }
        }
        this.order = null;
    }

  
    private void OnOrderAccepted(Order order)
    {
        if (arrowProgress)
        { 
            arrowProgress.enabled = true;
            order.OnChangeDeliveryTime += arrowProgress.SetProgress;
        }
        carOrderStatus.ShowOrder(order);
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
                    containers[i].amountLimit.range.y = order.items[j].quantity;
                    containers[i].enabled = true;
                    break;
                }
            }
        }

        order.OnAccepted += OnOrderAccepted;
        order.OnCompleted += OnRemoveOrder;
        order.OnFailed += OnRemoveOrder;
        order.OnRejected += OnRemoveOrder;
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
        if (maxArgs > 1)
            maxArgs = Random.Range(1, maxArgs + 1);

      
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

                var newOrder = OrderManagement.instance.GenerateNewOrder(args);
                OnAddingOrder(newOrder);
            }
            else
                yield return new WaitForSeconds(10);
        }
    }
}
