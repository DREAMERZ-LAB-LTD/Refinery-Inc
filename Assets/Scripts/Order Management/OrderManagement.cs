using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OrderManagement : MonoBehaviour
{
    [Header("References")]
    [SerializeReference] private OrderPanelButtonEventHandler orderManagementUI;
    [SerializeReference] private WareHouse wareHouse;

    [Header("Management Controls")]
    [SerializeField] private int timeSegment = 5;
    [SerializeField] private int maxPendingTime = 5;
    [SerializeField] private Vector2Int delayBetweenOrder = new Vector2Int (5, 10);
    [SerializeField] private Vector2Int itemCountRange = new Vector2Int(1, 2);
    [SerializeField] private Vector2Int quantityRange = new Vector2Int(1, 2);
    [SerializeField] private int maxActiveOrderCount = 1;
    [SerializeField] private int maxPendingOrderCount = 1;

    private List<Order> pendingOrders = new List<Order>();
    private List<Order> acceptedOrders = new List<Order>();

    [Header("Callback Events")]
    [SerializeField] private UnityEvent m_OnGenerateOrder;
    [SerializeField] private UnityEvent m_OnOrderAccepted;
    [SerializeField] private UnityEvent m_OnOrderCompleted;
    [SerializeField] private UnityEvent m_OnOnOrderFailed;

    private void Awake()
    {
        Client.availables.Clear();
        StartCoroutine(OrderGeneratorRoutine());
    }

    private void Update()
    {
        for (int i = 0; i < pendingOrders.Count; i++)
            pendingOrders[i].Update();

        for (int i = 0; i < acceptedOrders.Count; i++)
            acceptedOrders[i].Update();
    }

    private void OnOrderCompleted(Order order)
    {
        if (acceptedOrders.Contains(order))
        { 
            acceptedOrders.Remove(order);
            order.OnCompleted -= OnOrderCompleted;
            order.OnFailed -= OnOrderCompleted;
        }
        m_OnOrderCompleted.Invoke();
    }

    private void OnOrderFailed(Order order)
    {
        if (acceptedOrders.Contains(order))
        {
            acceptedOrders.Remove(order);
            order.OnCompleted -= OnOrderCompleted;
            order.OnFailed -= OnOrderFailed;
        }
        m_OnOnOrderFailed.Invoke();
    }

    private void OnOrderAccepted(Order order)
    {
        RemoveFromPending(order);

        if (!acceptedOrders.Contains(order))
        { 
            acceptedOrders.Add(order);
            order.OnCompleted += OnOrderCompleted;
            order.OnFailed += OnOrderFailed;
        }
        m_OnOrderAccepted.Invoke();
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
        var isValidAmount = acceptedOrders.Count < maxActiveOrderCount;
        orderManagementUI.AcceptBtn.interactable = isValidAmount;
    }

    public Order GenerateNewOrder()
    {
        var itemCount = Random.Range(itemCountRange.x, itemCountRange.y + 1);
        var tempItemSets = new List<Item.Identity>(Item.availables);
        var extraItems = tempItemSets.Count - itemCount;
        for (int i = 0; i < extraItems; i++)
            tempItemSets.RemoveAt(Random.Range(0, tempItemSets.Count));

        Order order = null;
        Item.Identity item;

        for (int i = 0; i < tempItemSets.Count; i++)
        { 
            item = tempItemSets[i];

            if (order == null)
                order = new Order();

            var quantity = Random.Range(quantityRange.x, quantityRange.y);
            var newItem = new Item.Identity(item.iD, item.name, item.price, quantity, item.icon);
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
            while (Client.availables.Count == 0)
                yield return new WaitForSeconds(2);
            
            while(pendingOrders.Count >= maxPendingOrderCount)
                yield return new WaitForSeconds(2);

            int delay = Random.Range(delayBetweenOrder.x, delayBetweenOrder.y);
            yield return new WaitForSeconds(delay);


           
            var newOrder = GenerateNewOrder();
            if (newOrder != null)
            {
                int index = Random.Range(0, Client.availables.Count);
                client = Client.availables[index];
                Client.availables.RemoveAt(index);

                var sp = wareHouse.sellsPoints[client.sellsPoint];
                newOrder.destination = sp.point.position;

                AddToPending(newOrder);
                newOrder.OnAccepted += wareHouse.OnOrderAccepted;
                newOrder.OnAccepted += client.OnOrderAccepted;
                newOrder.OnRejected += client.OnOrderCanceled;
                newOrder.OnCanceled += client.OnOrderCanceled;
                m_OnGenerateOrder.Invoke();
            }
        }
    }
}