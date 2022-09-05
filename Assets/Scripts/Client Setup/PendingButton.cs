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
        order.OnFailed += OnDestroy;
        order.OnRejected += OnDestroy;
        order.OnCompleted += OnDestroy;

    }

    private void OnDestroy()
    {
        pendingBtn.onClick.RemoveListener(OnClick);
        order.OnChangePendingTime -= OnUpdateOrderTime;
        order.OnChangeDeliveryTime -= OnUpdateOrderTime;
        order.OnFailed -= OnDestroy;
        order.OnRejected -= OnDestroy;
        order.OnCompleted -= OnDestroy;
    }

    public void OnDestroy(Order order)
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
