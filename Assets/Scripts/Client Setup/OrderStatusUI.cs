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
    private void OnOrderDispose(Order order) => SetPanelVisibility = false;

    public void ShowOrder(Order order)
    {
        SetPanelVisibility = true;

        if (lastClikcedOrder == order)
            return;

        if (lastClikcedOrder != null)
        {
            lastClikcedOrder.OnChangeDeliveryTime -= OnUpdateOrderTime;
            lastClikcedOrder.OnFailed -= OnOrderDispose;
            lastClikcedOrder.OnRejected -= OnOrderDispose;
        }

        lastClikcedOrder = order;
        order.OnChangeDeliveryTime += OnUpdateOrderTime;
        order.OnFailed += OnOrderDispose;
        order.OnRejected += OnOrderDispose;

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
                availableFields[i].SetInfo(null, message);
            }
            else
                availableFields[i].gameObject.SetActive(false);
        }
    }
}
