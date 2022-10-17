using Tutorial;

public class OrderButtonBlinkEffect : TriggerableTutorial
{
    public void OnNewOrderGenerated()
    {
        if (!enabled) return;

        FireEvent(0);

        if (!isTriggred)
        { 
            var exploreBtn = GetComponentsInChildren<PendingButton>();
            for (int i = 0; i < exploreBtn.Length; i++)
            {
                var scaleEffect = exploreBtn[i].GetComponent<ScalingEffect>();
                if (scaleEffect == null)
                    exploreBtn[i].gameObject.AddComponent<ScalingEffect>();
            }
        }
    }

    public void OnOrderAccepted()
    {
        if (!enabled) return;

        if (!isTriggred)
        { 
            var exploreBtn = GetComponentsInChildren<PendingButton>();
            for (int i = 0; i < exploreBtn.Length; i++)
            {
                var scaleEffect = exploreBtn[i].GetComponent<ScalingEffect>();
                if (scaleEffect)
                    Destroy(scaleEffect);
            }
        }
        FireEvent(1);
        OnTriggered();
    }
}
