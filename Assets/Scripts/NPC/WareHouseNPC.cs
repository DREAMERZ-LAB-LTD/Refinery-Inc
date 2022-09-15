using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using IdleArcade.Core;

public class WareHouseNPC : NPCBehaviour
{
    class NodePeer
    {
        public TransactionContainer selfContainer;
        public TransactionContainer wareHouseContainer;
        public Item.Identity itemIdentity;
    }

    [SerializeReference] TransactionContainer[] warehouseContainers;
    [Header("Movement Setup")]
    [SerializeReference] private NavMeshAgent agent;

    private bool isDelivering = false;
    private Vector3 homePoint;

    public static List<WareHouseNPC> availables = new List<WareHouseNPC>();
    private TransactionContainer GetWarehouseContainer(string id)
    {
        for (int i = 0; i < warehouseContainers.Length; i++)
            if (id == warehouseContainers[i].GetID)
                return warehouseContainers[i];

        return null;
    }

    #region NPC Registrtion
    protected override void Start()
    {
        base.Start();
        homePoint = transform.position;
        AddToAvailable();
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

    public void Assigned(Order order)
    {
        if (order == null)
            return;

        order.OnCompleted += OnOrderRemoved;
        order.OnFailed += OnOrderRemoved;

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

        Debug.Log("Total Node Peers " + nodePeers.Count);
        isDelivering = false;
        StopAllCoroutines();
        StartCoroutine(ServeRoutine(nodePeers, order.destination));
    }

    private void OnOrderRemoved(Order order)
    {
        order.OnCompleted -= OnOrderRemoved;
        order.OnFailed -= OnOrderRemoved;

        StopAllCoroutines();
        AddToAvailable();
        GoHome();
    }
    #endregion NPC Task Assigning



    #region Movement System


    private IEnumerator ServeRoutine(List<NodePeer> nodePeer, Vector3 destination)
    {
        if (nodePeer.Count == 0)
            yield break;

        for (int i = 0; i < nodePeer.Count; i++)
            nodePeer[i].selfContainer.enabled = true;

        var tempPeer = new List<NodePeer>(nodePeer);

        int peerNo = 0;
        bool sourceIsEmpty;
        bool hasLeft;
        while (tempPeer.Count > 0)
        {
            var peer = tempPeer[peerNo];
            sourceIsEmpty = peer.wareHouseContainer.isEmpty;
            hasLeft = peer.itemIdentity.quantity > peer.selfContainer.Getamount;

            if (!sourceIsEmpty && hasLeft)
                agent.SetDestination(peer.itemIdentity.pickPoint);
            

            while (!sourceIsEmpty && hasLeft)
            {
                sourceIsEmpty = peer.wareHouseContainer.isEmpty;
                hasLeft = peer.itemIdentity.quantity > peer.selfContainer.Getamount;
                yield return null;
            }

            if (!hasLeft)
            { 
                Debug.Log("Removed id" + peer.selfContainer.GetID);
                tempPeer.RemoveAt(peerNo);
                peerNo--;
            }
            
                Debug.Log("Collecting END Peer Count " + tempPeer.Count);
            
             peerNo++;
            if (tempPeer.Count > 0)
            {
                 agent.SetDestination(destination);
                isDelivering = peerNo == tempPeer.Count;
                peerNo = peerNo % tempPeer.Count;
            }


            while (isDelivering)
            {
                yield return null;
            }
            yield return null;
        }

        agent.SetDestination(destination);
    }
    private void GoHome()
    {
        agent.SetDestination(homePoint);
    }

    //private void Update()
    //{
    //    foreach (var c in containers)
    //        if (c.enabled)
    //            Debug.Log(c.GetID);
    //}

    protected override void OnElementAdding(string id, int currnet, int max)
    {
   
     //   Debug.Log("Adding");
    }


    protected override void OnElementRemoving(string id, int currnet, int max)
    {
     //   Debug.Log("Removing");
    }

    protected override void OnEmpty()
    {
        isDelivering = false;
        //   Debug.Log("Empty");
    }

    protected override void OnFilledUp()
    {
     
      //  Debug.Log("FilledUp");
    }
    #endregion Movement System
}
