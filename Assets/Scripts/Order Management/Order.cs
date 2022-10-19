using System.Collections.Generic;
using UnityEngine;

public class Order
{
    public delegate void OnOrder(Order order);
    public OnOrder OnAccepted;
    public OnOrder OnCompleted;
    public OnOrder OnFailed;
    public OnOrder OnRejected;
    public OnOrder OnCanceled;
    public OnOrder OnChangedValue;
    public delegate void OnProgressChange(float current, float max);
    public OnProgressChange OnChangePendingTime;
    public OnProgressChange OnChangeDeliveryTime;

    private float maxPendingTime;
    private float maxDeliveryTime;
    private float pendingTime;
    private float deliveryTime;
    public Vector3 destination;

    public bool isAccepted = false;
    public bool isRejected = false;
    public bool isShifting = false;

    public List<Item.Identity> items = new List<Item.Identity>();

    public Item.Identity GetItem(string id)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].iD == id)
                return items[i];

        return null;
    }

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
                    if (OnCanceled != null)
                        OnCanceled.Invoke(this);
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
    public bool FillUpItem(string id, int delta)
    {
        bool isCompleted = true;
        Item.Identity item;
        for (int i = 0; i < items.Count; i++)
        {
            item = items[i];
            if (item.iD == id)
            { 
                item.quantity -= delta;
                if (item.quantity < 0)
                    item.quantity = 0;

                if (OnChangedValue != null)
                    OnChangedValue.Invoke(this);
            }
            isCompleted &= item.quantity <= 0;
        }

        if (isCompleted)
            if (OnCompleted != null)
                OnCompleted.Invoke(this);

        return isCompleted;
    }
}