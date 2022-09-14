using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WareHouseNPC : NPCBehaviour
{
    [Header("Movement Setup")]
    [SerializeReference] private NavMeshAgent agent;
    private Vector3 homePoint;

    private List<Vector3> path = new List<Vector3>();
    private int pathPoint;

    public static List<WareHouseNPC> availables = new List<WareHouseNPC>();


    #region NPC Task Assigning
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

    private void ApplyContainerMask(Order order)
    {
        for (int i = 0; i < order.items.Count; i++)
            for (int j = 0; j < containers.Length; j++)
                containers[j].enabled = order.items[i].iD == containers[j].GetID;
    }

    public void Assigned(Order order, List<Vector3> path)
    {
        if (order == null)
            return;

        RemoveFromAvailable();
        ApplyContainerMask(order);

        this.path.Clear();
        this.path = path;

        order.OnCompleted += OnOrderRemoved;
        order.OnFailed += OnOrderRemoved;

        Debug.Log("path " + path.Count);
        agent.SetDestination(path[0]);
    }

    private void OnOrderRemoved(Order order)
    {
        GoHome();
        path.Clear();
        order.OnCompleted -= OnOrderRemoved;
        order.OnFailed -= OnOrderRemoved;

        AddToAvailable();
    }
    #endregion NPC Task Assigning

    private IEnumerator ForceToMove()
    {
        yield return new WaitForSeconds(2);
        pathPoint++;
        agent.SetDestination(path[pathPoint]);
    }


    #region Movement System

    private void GoHome()
    {
        agent.SetDestination(homePoint);
    }

    protected override void OnElementAdding()
    {
        StopAllCoroutines();
        StartCoroutine(ForceToMove());
        Debug.Log("Adding");
    }

    protected override void OnElementRemoving()
    {
        Debug.Log("Removing");
    }

    protected override void OnEmpty()
    {
        GoHome();
        Debug.Log("Empty");
    }

    protected override void OnFilledUp()
    {
        
        Debug.Log("FilledUp");
    }
    #endregion Movement System
}
