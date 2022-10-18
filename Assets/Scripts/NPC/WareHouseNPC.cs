using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using IdleArcade.Core;

public class WareHouseNPC : Containable
{
    [SerializeReference] Containable warehouseContainable;
    [SerializeReference] TransactionBridgeLimit bridgeLimit;

    [Header("Movement Setup")]
    [SerializeReference] private NavMeshAgent agent;
    [SerializeField] private PathNode[] collectingPoints;
    [SerializeField] public PathNode[] sellsPoints;

    private List<Order> shiftingOrders = new List<Order>();
    private bool isWorking = false;
    private Vector3 homePoint;


    #region NPC Initilization

    private void OnChangedValue(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        isWorking = true;
    }

    private void Start()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue += OnChangedValue;

        homePoint = transform.position;
    }


    private void OnDestroy()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue -= OnChangedValue;
    }
    private void OnEnable()
    {
        StartCoroutine(ServeRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    #endregion NPC Initilization



    #region NPC Task Assigning

    private void OnOrderRemoved(Order order)
    {
        order.OnCompleted -= OnOrderRemoved;
        order.OnFailed -= OnOrderRemoved;
        shiftingOrders.Remove(order);
    }

    public void ShiftOrder(Order order, int destinationPoint)
    {
        if (order == null)
            return;

        order.OnCompleted += OnOrderRemoved;
        order.OnFailed += OnOrderRemoved;

        for (int i = 0; i < order.items.Count; i++)
            for (int p = 0; p < collectingPoints.Length; p++)
                if (order.items[i].iD == collectingPoints[p].id)
                    order.items[i].pickPoint = collectingPoints[p].point.position;

        var sp = sellsPoints[destinationPoint];
        order.destination = sp.point.position;

        shiftingOrders.Add(order);
    }
    #endregion NPC Task Assigning


    #region Life Cycle
    private void GoHome()
    {
        agent.SetDestination(homePoint);
    }

    private IEnumerator ServeRoutine()
    {
        SetActiveContainers(false);

        while (true)
        {
            if (shiftingOrders.Count == 0)
            {
                GoHome();
                while (shiftingOrders.Count == 0)
                    yield return new WaitForSeconds(1.5f);
            }

            while (shiftingOrders.Count > 0)
            {
                //collecting
                for (int i = 0; i < shiftingOrders.Count; i++)
                {
                    for (int j = 0; j < shiftingOrders[i].items.Count; j++)
                    {
                        if (shiftingOrders[i].items[j].quantity > 0)
                        {
                            var selfCon = GetContainer(shiftingOrders[i].items[j].iD);
                            bool isLeft() => shiftingOrders[i].items[j].quantity - selfCon.Getamount > 0;
                            if (isLeft())
                            { 
                                var warehouseCon = warehouseContainable.GetContainer(shiftingOrders[i].items[j].iD);
                                bool isValidContainer() => !warehouseCon.isEmpty && !bridgeLimit.isFilledUp;
                         

                                if (isValidContainer())
                                {
                                    agent.SetDestination(shiftingOrders[i].items[j].pickPoint);
                                    yield return new WaitForSeconds(1);
                                    
                                    selfCon.enabled = true;

                                    while (isValidContainer() && isLeft())
                                        yield return null;
                                    
                                    SetActiveContainers(false);
                                }
                            }

                        }
                    }

                }


                if (shiftingOrders.Count > 0)
                {
                    TransactionContainer container;
                   SetActiveContainers(true);
                    for (int i = 0; i < shiftingOrders.Count; i++)
                    {
                        for (int j = 0; j < shiftingOrders[i].items.Count; j++)
                        {
                            if (shiftingOrders[i].items[j].quantity > 0)
                            { 
                                container = GetContainer(shiftingOrders[i].items[j].iD);
                                if(!container.isEmpty)
                                { 
                                    agent.SetDestination(shiftingOrders[i].destination);
                                    yield return new WaitForSeconds(1);
                        
                                    while (agent.velocity.magnitude > 0)
                                        yield return new WaitForSeconds(1);
                        

                                    while (isWorking)
                                    { 
                                        isWorking = false;
                                        yield return new WaitForSeconds(1.5f);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    SetActiveContainers(false);
                    GoHome();
                }
            
                yield return null;
            }
            yield return null;
        }

    }

    #endregion Life Cycle
}
