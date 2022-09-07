using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PendingButton : MonoBehaviour
{
    public Order.OnOrder OnPressed;

    public Button pendingBtn;
    public Image btnProgressBar;
    public Order order;

    private void Start()
    {
        pendingBtn.onClick.AddListener(OnClick);
        order.OnChangePendingTime += OnUpdateOrderTime;
        order.OnChangeDeliveryTime += OnUpdateOrderTime;
        order.OnAccepted += OrOrderFinished;
        order.OnCompleted += OrOrderFinished;
        order.OnFailed += OrOrderFinished;
        order.OnRejected += OrOrderFinished;
        order.OnCanceled += OrOrderFinished;

    }

    private void OnDestroy()
    {
        pendingBtn.onClick.RemoveListener(OnClick);
        order.OnChangePendingTime -= OnUpdateOrderTime;
        order.OnChangeDeliveryTime -= OnUpdateOrderTime;
        order.OnAccepted -= OrOrderFinished;
        order.OnCompleted -= OrOrderFinished;
        order.OnFailed -= OrOrderFinished;
        order.OnRejected -= OrOrderFinished;
        order.OnCanceled -= OrOrderFinished;
    }

    public void OrOrderFinished(Order order)
    {
        Destroy(gameObject);
    }

    private void OnClick()
    {
        if (OnPressed != null)
            OnPressed.Invoke(order);
    }

    private void OnUpdateOrderTime(float currnt, float max) => btnProgressBar.fillAmount = currnt / max;
    
}
