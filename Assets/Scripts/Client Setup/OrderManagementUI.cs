using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderManagementUI : MonoBehaviour
{
 
    [SerializeField] public Button AcceptBtn;
    [SerializeField] private Button RejectBtn;
    [SerializeField] private Image progressBar;

    [Header("panels References")]
    [SerializeField] private GameObject detailsPanel;

    [SerializeField] private PendingButton pendingBtnPrefab;
    [SerializeField] private Transform btnContainer;

    [SerializeField] private ItemFieldUI itemFieldPrefab;
    [SerializeField] private Transform fieldContainer;

    private Order lastClikcedOrder = null;

    private List<ItemFieldUI> availableFields = new List<ItemFieldUI>();

    public void OnCLickPendingButton(Order order)
    {
        detailsPanel.SetActive(true);

        if (lastClikcedOrder == order)
            return;

        if (lastClikcedOrder != null)
        {
            lastClikcedOrder.OnChangeDeliveryTime -= OnUpdateOrderTime;
            lastClikcedOrder.OnFailed -= OnOrderFailed;
        }

        lastClikcedOrder = order;
        order.OnChangeDeliveryTime += OnUpdateOrderTime;
        order.OnFailed += OnOrderFailed;

        if (AcceptBtn)
        {
            AcceptBtn.onClick.RemoveAllListeners();
            AcceptBtn.onClick.AddListener(order.Accept);
            AcceptBtn.onClick.AddListener(OnCickAcceptBtn);
            AcceptBtn.interactable = !order.isAccepted;
        }

        if (RejectBtn)
        {
            RejectBtn.onClick.RemoveAllListeners();
            RejectBtn.onClick.AddListener(order.Rejct);
            RejectBtn.onClick.AddListener(OnCickRejecttBtn);
            RejectBtn.interactable = !order.isAccepted;
        }

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
                var message = order.items[i].name + " "+ order.items[i].quantity.ToString();
                availableFields[i].SetInfo(null, message);
            }
            else
                availableFields[i].gameObject.SetActive(false);
        }
    }

    private void OnOrderFailed(Order order)
    {
        detailsPanel.SetActive(false);
    }

    private void OnCickAcceptBtn()
    {
        if (AcceptBtn)
            AcceptBtn.interactable = false;
        if (RejectBtn)
            RejectBtn.interactable = false;
    }
    private void OnCickRejecttBtn()
    {
        AcceptBtn.interactable = false;
        RejectBtn.interactable = false;
        detailsPanel.SetActive(false);
    }

    private void OnUpdateOrderTime(float currnt, float max) => progressBar.fillAmount = currnt / max;
    

    
    public void SpawnPendingButton(Order order, Order.OnOrder action)
    {
        var pendingBtn = Instantiate(pendingBtnPrefab.gameObject, btnContainer);
        pendingBtn.SetActive(true);
        var btn = pendingBtn.GetComponent<PendingButton>();
        btn.OnPressed += OnCLickPendingButton;
        if(action!= null)
            btn.OnPressed += action;
        btn.order = order;
    }
}
