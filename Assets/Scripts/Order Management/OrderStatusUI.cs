using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderStatusUI : MonoBehaviour
{
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private Image progressBar;
    [SerializeField] private ItemFieldUI itemFieldPrefab;
    [SerializeField] private Transform fieldContainer;

    private Order lastClikcedOrder = null;
    private List<ItemFieldUI> availableFields = new List<ItemFieldUI>();

    public bool SetPanelVisibility
    {
        set
        {
            detailsPanel.SetActive(value);
        }
    }
    private void OnUpdateOrderTime(float currnt, float max)
    { 
        if(progressBar)
            progressBar.fillAmount = currnt / max;
    }

    public void OnOrderUpdate(Order order)
    {
        for (int i = 0; i < order.items.Count; i++)
        {
            for (int j = 0; j < availableFields.Count; j++)
            {
                if (availableFields[j].id == order.items[i].iD)
                {
                    availableFields[j].quantityTxt.text = order.items[i].name + " " + order.items[i].quantity.ToString();
                }
            }
        }
    }

    private void OnOrderDispose(Order order) => SetPanelVisibility = false;

    public void ShowOrder(Order order)
    {
        SetPanelVisibility = true;

        if (lastClikcedOrder == order)
            return;

        if (lastClikcedOrder != null)
        {
            lastClikcedOrder.OnChangeDeliveryTime -= OnUpdateOrderTime;
            lastClikcedOrder.OnChangedValue -= OnOrderUpdate;
            lastClikcedOrder.OnFailed -= OnOrderDispose;
            lastClikcedOrder.OnRejected -= OnOrderDispose;
            lastClikcedOrder.OnCanceled -= OnOrderDispose;
            lastClikcedOrder.OnCompleted -= OnOrderDispose;
        }

        lastClikcedOrder = order;
        order.OnChangeDeliveryTime += OnUpdateOrderTime;
        order.OnChangedValue += OnOrderUpdate;
        order.OnFailed += OnOrderDispose;
        order.OnRejected += OnOrderDispose;
        order.OnCanceled += OnOrderDispose;
        order.OnCompleted += OnOrderDispose;

        int itemCount = order.items.Count;
        if (availableFields.Count < itemCount)
        {
            var remain = itemCount - availableFields.Count;
            for (int i = 0; i < remain; i++)
            {
                var fieldObj = Instantiate(itemFieldPrefab.gameObject, fieldContainer);
                var field = fieldObj.GetComponent<ItemFieldUI>();
                if (field)
                    availableFields.Add(field);
                else
                    Destroy(fieldObj);
            }
        }

        for (int i = 0; i < availableFields.Count; i++)
        {
            if (i < itemCount)
            {
                availableFields[i].gameObject.SetActive(true);
                var message = order.items[i].name + " " + order.items[i].quantity.ToString();
                availableFields[i].quantityTxt.text = message;
                //availableFields[i].icon.sprite  = null;
                availableFields[i].id = order.items[i].iD;
                availableFields[i].icon.sprite = order.items[i].icon;
            }
            else
                availableFields[i].gameObject.SetActive(false);
        }
    }
}
