using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using IdleArcade.Core;

public class WareHouseNPC : Containable
{
    class NodePeer
    {
        public TransactionContainer selfContainer;
        public TransactionContainer wareHouseContainer;
        public Item.Identity itemIdentity;
    }

    [SerializeReference] TransactionContainer[] warehouseContainers;
    [SerializeReference] TransactionBridgeLimit bridgeLimit;
    [Header("Movement Setup")]
    [SerializeReference] private NavMeshAgent agent;

    private bool isDelivering = false;
    private Vector3 homePoint;
    private Coroutine reCollectRoutine;


    public static List<WareHouseNPC> availables = new List<WareHouseNPC>();
    private TransactionContainer GetWarehouseContainer(string id)
    {
        for (int i = 0; i < warehouseContainers.Length; i++)
            if (id == warehouseContainers[i].GetID)
                return warehouseContainers[i];

        return null;
    }

    

    private void OnChangedValue(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {    
        if (delta < 0)
        {
            var isEmpty = true;
            for (int i = 0; i < containers.Length; i++)
                if (containers[i].enabled)
                    isEmpty &= containers[i].isEmpty;

            if (isEmpty)
                isDelivering = false;
        }
    }


    #region NPC Registrtion
    private void Start()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue += OnChangedValue;

        homePoint = transform.position;
        AddToAvailable();
    }
    private void OnDestroy()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue -= OnChangedValue;
    }
    private void OnEnable()
    {
        AddToAvailable();
    }
    private void OnDisable()
    {
        RemoveFromAvailable();
    }
    private void AddToAvailable()
    {
        if (!availables.Contains(this))
            availables.Add(this);
    }
    private void RemoveFromAvailable()
    {
        if (availables.Contains(this))
            availables.Remove(this);
    }
    #endregion NPC Registrtion


    #region NPC Task Assigning

    private void OnOrderUpdate(Order order)
    {
        if (reCollectRoutine != null)
            StopCoroutine(reCollectRoutine);

        reCollectRoutine = StartCoroutine(RecollectingRoutine());

        IEnumerator RecollectingRoutine()
        {
            yield return new WaitForSeconds(2f);
            isDelivering = false;
        }

    }
    private void OnOrderRemoved(Order order)
    {
        order.OnCompleted -= OnOrderRemoved;
        order.OnFailed -= OnOrderRemoved;
        order.OnChangedValue -= OnOrderUpdate;

        StopAllCoroutines();
        AddToAvailable();
        GoHome();
    }
    public void Assigned(Order order)
    {
        if (order == null)
            return;

        order.OnCompleted += OnOrderRemoved;
        order.OnFailed += OnOrderRemoved;
        order.OnChangedValue += OnOrderUpdate;

        RemoveFromAvailable();

        for (int i = 0; i < containers.Length; i++)
            containers[i].enabled = false;

        List<NodePeer> nodePeers = new List<NodePeer>();
        TransactionContainer warHouseCont;
        TransactionContainer SelfCont;
        Item.Identity itemIdentity;
        for (int i = 0; i < order.items.Count; i++)
        {
            itemIdentity = order.items[i];
            warHouseCont = GetWarehouseContainer(itemIdentity.iD);
            SelfCont = GetContainer(itemIdentity.iD);

            var ns = new NodePeer();
            ns.itemIdentity = itemIdentity;
            ns.selfContainer = SelfCont;
            ns.wareHouseContainer = warHouseCont;
            nodePeers.Add(ns);
        }

        isDelivering = false;
        StopAllCoroutines();
        StartCoroutine(ServeRoutine(nodePeers, order.destination));
    }



    #endregion NPC Task Assigning


    #region Movement System


    private IEnumerator ServeRoutine(List<NodePeer> nodePeers, Vector3 destination)
    {
        if (nodePeers.Count == 0)
            yield break;

        for (int i = 0; i < nodePeers.Count; i++)
            nodePeers[i].selfContainer.enabled = true;


        var tempPeer = new List<NodePeer>(nodePeers);
        int peerNo = 0;
        bool sourceIsEmpty;
        bool hasLeft;
        bool hasAvailableToDeliver = false;
        while (tempPeer.Count > 0)
        {
            var peer = tempPeer[peerNo];
            sourceIsEmpty = peer.wareHouseContainer.isEmpty;
            hasLeft = peer.itemIdentity.quantity > peer.selfContainer.Getamount;

            if (!sourceIsEmpty && hasLeft)
                agent.SetDestination(peer.itemIdentity.pickPoint);
            

            if (agent.hasPath)
            {
                for (int i = 0; i < nodePeers.Count; i++)
                    nodePeers[i].selfContainer.enabled = false;

                while (agent.hasPath)
                    yield return null;

                for (int i = 0; i < nodePeers.Count; i++)
                    nodePeers[i].selfContainer.enabled = true;
            }

            while (!sourceIsEmpty && hasLeft && !bridgeLimit.isFilledUp)
            {
                hasAvailableToDeliver = true;
                sourceIsEmpty = peer.wareHouseContainer.isEmpty;
                hasLeft = peer.itemIdentity.quantity > peer.selfContainer.Getamount;
                yield return null;
            }

            if (!hasLeft)
            {
                hasAvailableToDeliver = true;
                tempPeer.RemoveAt(peerNo);
                peerNo--;
            }
            
             peerNo++;
            if (tempPeer.Count > 0)
            {
                if (peerNo == tempPeer.Count)
                { 
                    isDelivering = hasAvailableToDeliver;
                    hasAvailableToDeliver = false;
                }

                peerNo = peerNo % tempPeer.Count;
            }
            if (isDelivering)
            { 
                agent.SetDestination(destination);
                while (agent.hasPath)
                    yield return null;

                for (int i = 0; i < nodePeers.Count; i++)
                    nodePeers[i].selfContainer.enabled = true;

                while (isDelivering)
                    yield return null;
                GoHome();
            }
            yield return null;
        }

        for (int i = 0; i < nodePeers.Count; i++)
            nodePeers[i].selfContainer.enabled = true;
        agent.SetDestination(destination);
    }
    private void GoHome()
    {
        agent.SetDestination(homePoint);
    }
    #endregion Movement System
}
