using UnityEngine;
using IdleArcade.Core;
using UnityEngine.Events;
using System.Collections.Generic;

public class Client : MonoBehaviour
{
    [SerializeField] private int positiveReview = 5;
    [SerializeField] private int negativeReview = -5;

    public int sellsPoint = 0;
    public static List<Client> availables = new List<Client>();

    [SerializeField] private TargetWithProgress arrowProgress;
    [SerializeField] private OrderStatusUI carOrderStatus;

    [SerializeReference]private TransactionContainer cacheContaier;
    [SerializeField] private TransactionContainer warehouseCoinContaier;
    [SerializeField]private TransactionContainer[] containers;

    private Order order = null;
    private ClientCar car;

    [Header("Callback Events")]
    [SerializeField] private UnityEvent m_ShiftBegin;
    [SerializeField] private UnityEvent OnOrderCompleted;
    [SerializeField] private UnityEvent OnOrderFailed;

    private void Start()
    {
        ApplyContainerMask();

        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue += OnTransact;

        car = GetComponent<ClientCar>();
        car.OnExportSide += AddToAvailable;
        car.OnImportSide += ApplyContainerMask;
    }
    private void OnDestroy()
    {
        for (int i = 0; i < containers.Length; i++)
            containers[i].OnChangedValue -= OnTransact;

        car.OnExportSide -= AddToAvailable;
        car.OnImportSide -= ApplyContainerMask;
    }

    private void OnEnable()
    {
        AddToAvailable();
    }

    private void OnDisable()
    {
        if (availables.Contains(this))
            availables.Remove(this);
    }
    private void AddToAvailable()
    {
        if (!availables.Contains(this))
            availables.Add(this);
    }


    private void OnTransact(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        if (order != null)
            order.FillUpItem(B.GetID, delta);
    }

    private void ApplyContainerMask()
    {
        for (int i = 0; i < containers.Length; i++)
        {
            containers[i].enabled = false;
            containers[i].amountLimit.range.y = 0;
            if (order != null)
            { 
                for (int j = 0; j < order.items.Count; j++)
                { 
                    if (containers[i].GetID == order.items[j].iD)
                    {
                        containers[i].amountLimit.range.y = order.items[j].quantity;
                        containers[i].enabled = true;
                        break;
                    }
                }
            }
        }
    }

    public void ShiftOrder(Order newOrder)
    {
        if (arrowProgress)
        {
            arrowProgress.enabled = true;
            newOrder.OnChangeDeliveryTime += arrowProgress.SetProgress;
        }
        newOrder.OnCompleted += OnCompleted;
        newOrder.OnFailed += OnFailed;
        order = newOrder;

        carOrderStatus.ShowOrder(order);
        m_ShiftBegin.Invoke();
    }

    private void OnCompleted(Order order)
    {
        order.OnCompleted -= OnCompleted;
        order.OnFailed -= OnFailed;

        if (arrowProgress)
        {
            order.OnChangeDeliveryTime -= arrowProgress.SetProgress;
            arrowProgress.enabled = false;
        }

        this.order = null;
        ApplyContainerMask();
        OnOrderCompleted.Invoke();

        int total = 0;
        for (int i = 0; i < order.items.Count; i++)
            total += order.items[i].price;

        cacheContaier.Add(total);
        warehouseCoinContaier.TransactFrom(total, cacheContaier);

        GameManager.instance.playerExprence.AddReview(positiveReview);
    }

    private void OnFailed(Order order)
    {
        order.OnCompleted -= OnCompleted;
        order.OnFailed -= OnFailed;


        if (arrowProgress)
        {
            order.OnChangeDeliveryTime -= arrowProgress.SetProgress;
            arrowProgress.enabled = false;
        }
        this.order = null;

        ApplyContainerMask();
        OnOrderFailed.Invoke();

        GameManager.instance.playerExprence.AddReview(negativeReview);
    }
}
