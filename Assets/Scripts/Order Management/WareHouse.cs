using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WareHouse : MonoBehaviour
{
    [SerializeField] private PathNode[] itemPoints;
    [SerializeField] public PathNode[] sellsPoints;
    private List<Order> shiftingOrders = new List<Order>();
    private void Awake()
    {
        WareHouseNPC.availables.Clear();
        StartCoroutine(SellingRoutine());
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void ShiftOrder(Order order)
    {
        if (order == null)
            return;

        for (int i = 0; i < order.items.Count; i++)
        {
            GetPickPoint(order.items[i].iD, out Vector3 pickPoint);
            order.items[i].pickPoint = pickPoint;
        }
        
        shiftingOrders.Add(order);

        order.OnCompleted += OnOrderRemoved;
        order.OnFailed += OnOrderRemoved;
    }


    private void OnOrderRemoved(Order order)
    {
        if (!shiftingOrders.Contains(order))
            return;
        shiftingOrders.Remove(order);

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
            while (WareHouseNPC.availables.Count > 0 && shiftingOrders.Count > 0)
            {
                var order = shiftingOrders[index];
                var npc = WareHouseNPC.availables[index];
                WareHouseNPC.availables.RemoveAt(index);
                shiftingOrders.RemoveAt(index);

                npc.Assigned(order);
                yield return null;
            }

            yield return new WaitForSeconds(2);
        }
    }
}
