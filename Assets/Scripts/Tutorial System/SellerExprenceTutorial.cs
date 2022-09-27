using UnityEngine;
using UnityEngine.Events;

public class SellerExprenceTutorial : MonoBehaviour
{
    [SerializeField] private PlayerExprence playerExprence;
    [SerializeField] private string id;
    [SerializeField] private int targetLevel = 1;
    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnLevel;
    public bool IsInvoked
    {
        get { return PlayerPrefs.GetInt(id) == 1; }
        set { PlayerPrefs.SetInt(id, value ? 1 : 0); }
    }


    private void Awake()
    {
       playerExprence.Progress = 0;
       playerExprence.Level = 0;

        playerExprence.OnChangeLevel += OnChangeEcperience;
        if (playerExprence.Level >= targetLevel)
            OnLevel.Invoke();
    }
    private void OnDestroy()
    {
        playerExprence.OnChangeLevel -= OnChangeEcperience;
    }
    public void OnChangeEcperience(int level)
    {
        if (targetLevel == level)
        {
            if (!IsInvoked)
            {
                IsInvoked = true;
                OnLevel.Invoke();
            }
        }
    }
}
