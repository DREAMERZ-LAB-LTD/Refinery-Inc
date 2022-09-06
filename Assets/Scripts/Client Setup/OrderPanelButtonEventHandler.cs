using UnityEngine;
using UnityEngine.UI;

public class OrderPanelButtonEventHandler : MonoBehaviour
{
    [SerializeField] private OrderStatusUI orderStatus;
 
    [SerializeField] public Button AcceptBtn;
    [SerializeField] private Button RejectBtn;
    [SerializeField] private Image progressBar;

    [SerializeField] private PendingButton exploreBtnPrefab;
    [SerializeField] private Transform btnContainer;


    private Order lastClikcedOrder = null;


    public void OnClickDetailButton(Order order)
    {
        if (lastClikcedOrder == order)
            return;

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

        orderStatus.ShowOrder(order);
    }


    private void OnCickAcceptBtn()
    {
        orderStatus.SetPanelVisibility = false;
        if (AcceptBtn)
            AcceptBtn.interactable = false;
        if (RejectBtn)
            RejectBtn.interactable = false;
    }
    private void OnCickRejecttBtn()
    {
        AcceptBtn.interactable = false;
        RejectBtn.interactable = false;
    }

    public void SpawnPendingButton(Order order, Order.OnOrder action)
    {
        var pendingBtn = Instantiate(exploreBtnPrefab.gameObject, btnContainer);
        pendingBtn.SetActive(true);
        var btn = pendingBtn.GetComponent<PendingButton>();
        btn.OnPressed += OnClickDetailButton;
        if(action!= null)
            btn.OnPressed += action;
        btn.order = order;
    }
}
