using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RawGarbageVisualizer : MonoBehaviour
{
    [System.Serializable]
    private class EventSet
    {
        public int targetOrderNo;
        public UnityEvent willInvoke;
        public UnityEvent alreadyInvoked;
    }
    [SerializeField] private OrderManagement orderManagement;
    [SerializeField] private Image progressBar;


    [SerializeField] private EventSet[] eventSets;

    private void Start()
    {
        orderManagement.OnCompleteOrder += OnOrderCompleted;
        orderManagement.OnCompleteOrder += UpdateProgressBarUI;

        int completedOrder = orderManagement.CompletedOrderCount;
        UpdateProgressBarUI(completedOrder);

        for (int i = 0; i < eventSets.Length; i++)
            if (eventSets[i].targetOrderNo <= completedOrder)
                eventSets[i].alreadyInvoked.Invoke();
    }

    private void OnDestroy()
    {
        orderManagement.OnCompleteOrder -= OnOrderCompleted;
        orderManagement.OnCompleteOrder -= UpdateProgressBarUI;
    }
    private void OnOrderCompleted(int completedOrderCount)
    {
        for (int i = 0; i < eventSets.Length; i++)
            if (eventSets[i].targetOrderNo == completedOrderCount)
                eventSets[i].willInvoke.Invoke();

    }

    private void UpdateProgressBarUI(int completedOrderCount)
    {
        if (eventSets.Length == 0)
        {
#if UNITY_EDITOR
            Debug.Log("<color=cyan> There are now any event set to measure garbage progress </color>");
#endif
            return;
        }

        if (progressBar)
            progressBar.fillAmount = Mathf.InverseLerp(0, eventSets[eventSets.Length - 1].targetOrderNo, completedOrderCount);
    }
}
