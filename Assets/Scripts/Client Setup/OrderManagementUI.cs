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
    [SerializeField] private Transform container;

    private Order lastClikcedOrder = null;

    public void OnCLickPendingButton(Order order)
    {
        detailsPanel.SetActive(true);

        if (lastClikcedOrder != order)
        {
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
        var pendingBtn = Instantiate(pendingBtnPrefab.gameObject, container);
        pendingBtn.SetActive(true);
        var btn = pendingBtn.GetComponent<PendingButton>();
        btn.OnPressed += OnCLickPendingButton;
        if(action!= null)
            btn.OnPressed += action;
        btn.order = order;
    }
}
