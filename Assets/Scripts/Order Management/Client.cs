using UnityEngine;
using IdleArcade.Core;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Client : MonoBehaviour
{
    public int sellsPoint = 0;
    public static List<Client> availables = new List<Client>();

    [SerializeField] private TargetWithProgress arrowProgress;
    [SerializeField] private OrderStatusUI carOrderStatus;

    [SerializeReference]private TransactionContainer cacheContaier;
    [SerializeField]private TransactionContainer[] containers;
    private WareHouseCoinContainer warehouseCoinContaier;

    private Order order;
    private ClientCar car;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent m_OnOrderAccepted;
    [SerializeField] private UnityEvent m_OnOrderRemoved;

    private void Start()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue += OnTransact;

        warehouseCoinContaier = FindObjectOfType<WareHouseCoinContainer>();
        car = GetComponent<ClientCar>();
        car.OnExportSide += AddToAvailable;
        car.OnImportSide += ApplyContainerMask;
    }

    private void OnEnable()
    {
        StartCoroutine(InitialDealyRoutine());
        IEnumerator InitialDealyRoutine()
        {
            yield return new WaitForEndOfFrame();
            AddToAvailable();
        }
    }

    private void OnDisable()
    {
        RemoveFromAvailable();
    }
    private void OnDestroy()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue -= OnTransact;

        car.OnExportSide -= AddToAvailable;
        car.OnImportSide -= ApplyContainerMask;
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


    private void OnTransact(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if (order != null)
            order.FillUpItem(B.GetID, delta);
    }

    public void OnOrderCanceled(Order order)
    {
        AddToAvailable();
    }

    public void OnCompleted(Order order)
    {
        order.OnCompleted -= OnCompleted;
        order.OnFailed -= OnFailed;

        order.OnAccepted -= OnOrderAccepted;
        if (arrowProgress)
        {
            order.OnChangeDeliveryTime -= arrowProgress.SetProgress;
            arrowProgress.enabled = false;
        }
        this.order = null;

        ApplyContainerMask();
        m_OnOrderRemoved.Invoke();

        int total = 0;
        for (int i = 0; i < order.items.Count; i++)
            total += order.items[i].price;

        cacheContaier.Add(total);
        warehouseCoinContaier.TransactFrom(total, cacheContaier);
    }

    public void OnFailed(Order order)
    {
        order.OnCompleted -= OnCompleted;
        order.OnFailed -= OnFailed;


        order.OnAccepted -= OnOrderAccepted;
        if (arrowProgress)
        {
            order.OnChangeDeliveryTime -= arrowProgress.SetProgress;
            arrowProgress.enabled = false;
        }
        this.order = null;

        ApplyContainerMask();
        m_OnOrderRemoved.Invoke();
    }

    public void OnOrderAccepted(Order order)
    {
        ApplyContainerMask();
        if (arrowProgress)
        { 
            arrowProgress.enabled = true;
            order.OnChangeDeliveryTime += arrowProgress.SetProgress;
        }

        order.OnCompleted += OnCompleted;
        order.OnFailed += OnFailed;
        this.order = order;

        carOrderStatus.ShowOrder(order);
        m_OnOrderAccepted.Invoke();
    }

    private void ApplyContainerMask()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].enabled = false;
            if(order != null)
                for (int j = 0; j < order.items.Count; j++)
                    if (containers[i].GetID == order.items[j].iD)
                    {
                        containers[i].amountLimit.range.y = order.items[j].quantity;
                        containers[i].enabled = true;
                        break;
                    }
        }
    }

}
