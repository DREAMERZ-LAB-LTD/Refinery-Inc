using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WareHouse : MonoBehaviour
{
    [SerializeField] private PathNode[] itemPoints;
    [SerializeField] public PathNode[] sellsPoints;
    private List<Order> acceptedOrders = new List<Order>();
    private void Awake()
    {
        WareHouseNPC.availables.Clear();
        StartCoroutine(SellingRoutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void OnOrderAccepted(Order order)
    {
        if (order == null)
            return;

        for (int i = 0; i < order.items.Count; i++)
        {
            GetPickPoint(order.items[i].iD, out Vector3 pickPoint);
            order.items[i].pickPoint = pickPoint;
        }
        
        acceptedOrders.Add(order);

        order.OnAccepted -= OnOrderAccepted;
        order.OnCompleted += OnOrderRemoved;
        order.OnFailed += OnOrderRemoved;
    }


    private void OnOrderRemoved(Order order)
    {
        if (!acceptedOrders.Contains(order))
            return;
        acceptedOrders.Remove(order);

        order.OnCompleted -= OnOrderRemoved;
        order.OnFailed -= OnOrderRemoved;
    }

    private bool GetPickPoint(in string id, out Vector3 point)
    {
        point = Vector3.zero;
        for (int i = 0; i < itemPoints.Length; i++)
            if (id == itemPoints[i].id) 
            {
                point = itemPoints[i].point.position;
                return true;
            }

        return false;
    }

    private IEnumerator SellingRoutine()
    {
        while (true)
        {
            while (WareHouseNPC.availables.Count == 0)
                yield return new WaitForSeconds(2);

            int index = 0;
            while (WareHouseNPC.availables.Count > 0 && acceptedOrders.Count > 0)
            {
                var order = acceptedOrders[index];
                var npc = WareHouseNPC.availables[index];
                WareHouseNPC.availables.RemoveAt(index);
                acceptedOrders.RemoveAt(index);

                npc.Assigned(order);
                yield return null;
            }

            yield return new WaitForSeconds(2);
        }
    }
}
