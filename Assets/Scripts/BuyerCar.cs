using UnityEngine;
using IdleArcade.Core;

public class BuyerCar : MonoBehaviour
{
    public TransactionContainer carContainer;
    public TransactionVisualCore visual;

    public void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            carContainer.Add(carContainer.Getamount);
            visual.Clear();
        }
    }




}
