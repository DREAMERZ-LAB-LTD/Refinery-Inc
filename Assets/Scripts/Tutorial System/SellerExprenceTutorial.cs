using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SellerExprenceTutorial : MonoBehaviour
{
    [SerializeField] private string id;
    [SerializeField] private int targetLevel = 1;
    [Header("Callback Events")]
    [SerializeField] private UnityEvent OnLevel;
    public bool IsInvoked
    {
        get { return PlayerPrefs.GetInt(id) == 1; }
        set { PlayerPrefs.SetInt(id, value ? 1 : 0); }
    }


    private void Start()
    {
        StartCoroutine(SkipFrame());
        IEnumerator SkipFrame()
        {
            yield return new WaitForEndOfFrame();
            GameManager.instance.playerExprence.OnChangeLevel += OnChangeEcperience;
        }
    }
    private void OnDestroy()
    {
        if(GameManager.instance)
            GameManager.instance.playerExprence.OnChangeLevel -= OnChangeEcperience;
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
