using UnityEngine;
using UnityEngine.UI;

public class SlackingAroundStatus : MonoBehaviour
{
    private void Awake()
    {
        var canvuses = FindObjectsOfType<Canvas>();
        if (canvuses != null)
            for (int i = 0; i < canvuses.Length; i++)
                if (canvuses[i].renderMode == RenderMode.ScreenSpaceOverlay)
                    transform.parent = canvuses[i].transform;
    }
    public Button wakeUpBtn;
    public Image peogressBar;
}
