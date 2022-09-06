using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemFieldUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityTxt;

    public void SetInfo(Image icon, string text)
    {
       // this.icon = icon;
        quantityTxt.text = text;
    }
}
