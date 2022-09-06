using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManagement : MonoBehaviour
{
    #region Singleton
    private static OrderManagement _instance = null;
    public static OrderManagement instance => _instance;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;

        if (_instance != this)
            Destroy(gameObject);
        
    }
    private void OnDestroy()
    {
        if(_instance == this)
            _instance = null;
    }
    #endregion


    [SerializeField] private OrderPanelButtonEventHandler orderManagementUI;

    [SerializeField] private int timeSegment = 5;
    [SerializeField] private int maxPendingTime = 5;
    [SerializeField] private Vector2Int quantityRange = new Vector2Int(1, 2);
    [SerializeField] private int maxActiveOrderCount = 1;
    [SerializeField] private int maxPendingOrderCount = 1;

    [SerializeField] private Item[] itemSets;
    public List<Order> pendingOrders = new List<Order>();
    public List<Order> activeOrders = new List<Order>();

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
            order.OnFailed -= RemoveFromActive;
        }
    }

    private void AddToActive(Order order)
    {
        if (!activeOrders.Contains(order))
        { 
            activeOrders.Add(order);
            order.OnFailed += RemoveFromActive;
        }
    }

    private void RemoveFromPending(Order order)
    {
        if (pendingOrders.Contains(order))
        { 
            pendingOrders.Remove(order);

            order.OnFailed -= RemoveFromPending;
            order.OnRejected -= RemoveFromPending;
            order.OnAccepted -= RemoveFromPending;
            order.OnAccepted -= AddToActive;
        }
    }

    private void AddToPending(Order order)
    {
        if (!pendingOrders.Contains(order))
        { 
            pendingOrders.Add(order);

            order.OnFailed += RemoveFromPending;
            order.OnRejected += RemoveFromPending;
            order.OnAccepted += RemoveFromPending;
            order.OnAccepted += AddToActive;
            orderManagementUI.SpawnPendingButton(order, OnClickPendingButton);
        }
    }

    private void OnClickPendingButton(Order order)
    {
        var isValidAmount = activeOrders.Count < maxActiveOrderCount;
        orderManagementUI.AcceptBtn.interactable = isValidAmount;
    }

    public Order GenerateNewOrder(List<string> ids)
    {
        if (pendingOrders.Count >= maxPendingOrderCount)
            return null;

        Order order = null;
        Item item;
        for (int i = 0; i < ids.Count; i++)
        { 
            for (int j = 0; j < itemSets.Length; j++)
            {
                item = itemSets[j];
                if (item.iD == ids[i])
                {
                    var newItem = new Item();
                    newItem.iD = item.iD;
                    newItem.name = item.name;
                    newItem.quantity = Random.Range(quantityRange.x, quantityRange.y);

                    if(order == null)
                        order = new Order();

                    order.items.Add(newItem);
                }
            }
        }
        if (order != null)
        { 
            AddToPending(order);
            order.SetTime(timeSegment, maxPendingTime);
        }
        return order;
    }
}