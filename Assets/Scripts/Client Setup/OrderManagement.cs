using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManagement : MonoBehaviour
{
    [Header("Management Controls")]
    [SerializeField] private int timeSegment = 5;
    [SerializeField] private int maxPendingTime = 5;
    [SerializeField] private Vector2Int delayBetweenOrder = new Vector2Int (5, 10);
    [SerializeField] private Vector2Int itemCountRange = new Vector2Int(1, 2);
    [SerializeField] private Vector2Int quantityRange = new Vector2Int(1, 2);
    [SerializeField] private int maxActiveOrderCount = 1;
    [SerializeField] private int maxPendingOrderCount = 1;

    [Header("References")]
    [SerializeField] private OrderPanelButtonEventHandler orderManagementUI;
    [SerializeField] private Item[] itemSets;
    public List<Order> pendingOrders = new List<Order>();
    public List<Order> activeOrders = new List<Order>();
    public static List<Client> availableClients = new List<Client>();


    private void Awake()
    {
        availableClients.Clear();
        StartCoroutine(OrderGeneratorRoutine());
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        itemCountRange.x = Mathf.Clamp(itemCountRange.x, 1, itemSets.Length);
        itemCountRange.y = Mathf.Clamp(itemCountRange.y, itemCountRange.x, itemSets.Length);
    }
#endif    

    private void Update()
    {
        for (int i = 0; i < pendingOrders.Count; i++)
            pendingOrders[i].Update();

        for (int i = 0; i < activeOrders.Count; i++)
            activeOrders[i].Update();
    }

    private void RemoveFromActive(Order order)
    {
        if (activeOrders.Contains(order))
        { 
            activeOrders.Remove(order);
            order.OnCompleted -= RemoveFromActive;
            order.OnFailed -= RemoveFromActive;
        }
    }

    private void OnOrderAccepted(Order order)
    {
        RemoveFromPending(order);

        if (!activeOrders.Contains(order))
        { 
            activeOrders.Add(order);
            order.OnCompleted += RemoveFromActive;
            order.OnFailed += RemoveFromActive;
        }
    }

    private void RemoveFromPending(Order order)
    {
        if (pendingOrders.Contains(order))
        { 
            pendingOrders.Remove(order);

            order.OnAccepted -= OnOrderAccepted;
            order.OnRejected -= RemoveFromPending;
            order.OnCanceled -= RemoveFromPending;
        }
    }

    private void AddToPending(Order order)
    {
        if (!pendingOrders.Contains(order))
        { 
            pendingOrders.Add(order);

            order.OnAccepted += OnOrderAccepted;
            order.OnRejected += RemoveFromPending;
            order.OnCanceled += RemoveFromPending;
            orderManagementUI.SpawnPendingButton(order, OnClickPendingButton);
        }
    }

    private void OnClickPendingButton(Order order)
    {
        var isValidAmount = activeOrders.Count < maxActiveOrderCount;
        orderManagementUI.AcceptBtn.interactable = isValidAmount;
    }

    public Order GenerateNewOrder(int itemCount)
    {
        List<Item> tempItemSets = new List<Item>(itemSets);
        var extraItems = tempItemSets.Count - itemCount;
        for (int i = 0; i < extraItems; i++)
            tempItemSets.RemoveAt(Random.Range(0, tempItemSets.Count));

        Order order = null;
        Item item;

        for (int i = 0; i < tempItemSets.Count; i++)
        { 
            item = tempItemSets[i];
           
            var newItem = new Item();

            newItem.iD = item.iD;
            newItem.name = item.name;
            newItem.quantity = Random.Range(quantityRange.x, quantityRange.y);

            if (order == null)
                order = new Order();

            order.items.Add(newItem);
        }
        if (order != null)
            order.SetTime(timeSegment, maxPendingTime);
        
        return order;
    }


    private IEnumerator OrderGeneratorRoutine()
    {
        Client client = null;
        while (true)
        {
            while (availableClients.Count == 0)
                yield return new WaitForSeconds(2);
            
            while(pendingOrders.Count >= maxPendingOrderCount)
                yield return new WaitForSeconds(2);

            int delay = Random.Range(delayBetweenOrder.x, delayBetweenOrder.y);
            yield return new WaitForSeconds(delay);


            var itemCount = Random.Range(itemCountRange.x, itemCountRange.y + 1);
            var newOrder = GenerateNewOrder(itemCount);
            if (newOrder != null)
            {
                int index = Random.Range(0, availableClients.Count);
                client = availableClients[index];
                availableClients.RemoveAt(index);

                AddToPending(newOrder);
                newOrder.OnAccepted += client.OnOrderAccepted;
                newOrder.OnRejected += client.OnOrderCanceled;
                newOrder.OnCanceled += client.OnOrderCanceled;
            }
        }
    }
}