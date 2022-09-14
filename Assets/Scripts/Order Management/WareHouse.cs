using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WareHouse : MonoBehaviour
{
    [System.Serializable]
    private class PathNode
    {
        public string id;
        public Transform point;

    }
    private List<Order> acceptedOrders = new List<Order>();
    [SerializeField] private PathNode[] pathNodes;
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

    private List<Vector3> GetPath(Order order)
    {
        List<Vector3> path = new List<Vector3>();

        for (int i = 0; i < order.items.Count; i++)
            for (int j = 0; j < pathNodes.Length; j++)
                if (order.items[i].iD == pathNodes[j].id)
                    path.Add(pathNodes[j].point.position);

        path.Add(order.location);

        return path;
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

                var path = GetPath(order);
                npc.Assigned(order, path);
                yield return null;
            }

            yield return new WaitForSeconds(2);
        }
    }
}
