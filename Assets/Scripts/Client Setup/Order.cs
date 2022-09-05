using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public delegate void OnOrder(Order order);
    public OnOrder OnAccepted;
    public OnOrder OnRejected;
    public OnOrder OnFailed;
    public OnOrder OnCompleted;
    public OnOrder OnChangedValue;
    public delegate void OnProgressChange(float current, float max);
    public OnProgressChange OnChangePendingTime;
    public OnProgressChange OnChangeDeliveryTime;

    public string orderID;


    private float maxPendingTime;
    private float maxDeliveryTime;
    private float pendingTime;
    private float deliveryTime;

    public bool isAccepted = false;
    public bool isRejected = false;

    public List<Item> items = new List<Item>();


    public void SetTime(int timeSegment, int pendingTime )
    {
        int elementCount = 0;
        for (int i = 0; i < items.Count; i++)
            elementCount += items[i].quantity;

        deliveryTime = maxDeliveryTime = elementCount * timeSegment;
        this.pendingTime = maxPendingTime = pendingTime;
    }

    public void Accept()
    {
        if (!isRejected && !isAccepted)
        {
            isAccepted = true;
            if (OnAccepted != null)
                OnAccepted.Invoke(this);
        }
    }
    public void Rejct()
    {
        if (!isRejected)
        {
            isRejected = true;
            if (OnRejected != null)
                OnRejected.Invoke(this);
        }
    }
    public void Update()
    {
        if (!isRejected && !isAccepted)
        {
            if (pendingTime > 0)
            { 
                pendingTime -= Time.deltaTime;
                if (pendingTime <= 0)
                    pendingTime = 0;

                if (OnChangePendingTime != null)
                    OnChangePendingTime.Invoke(pendingTime, maxPendingTime);

                if (pendingTime == 0)
                {
                    isRejected = true;
                    if (OnFailed != null)
                        OnFailed.Invoke(this);
                }
            }
        }
        else if (isAccepted && deliveryTime > 0)
        {
            deliveryTime -= Time.deltaTime;
            if (deliveryTime <= 0)
                deliveryTime = 0;

            if (OnChangeDeliveryTime != null)
                OnChangeDeliveryTime.Invoke(deliveryTime, maxDeliveryTime);

            if (deliveryTime == 0)
            {
                isRejected = true;
                if (OnFailed != null)
                    OnFailed.Invoke(this);
            }
        }
    }

    /// <summary>
    /// Return true if order already completed
    /// </summary>
    /// <param name="id">item ID</param>
    /// <param name="delta">delta quantity change amount</param>
    /// <returns></returns>
    public bool FillUpItem(string id, int delta = 1)
    {
        bool isNotCompleted = true;
        Item item;
        for (int i = 0; i < items.Count; i++)
        {
            item = items[i];
            if (item.iD == id)
            { 
                item.quantity -= delta;

                if (OnCompleted != null)
                    OnChangedValue.Invoke(this);
            }
            isNotCompleted &= item.quantity > 0;
        }

        if (isNotCompleted == false)
            if (OnCompleted != null)
                OnCompleted.Invoke(this);

        return !isNotCompleted;
    }

}