using UnityEngine;
using IdleArcade.Core;
using UnityEngine.UI;
using TMPro;

public class PlayerExprence : Limiter
{
    public interface IExprenceLevel
    {
        public void OnChangeEcperience(int level);
    }

    [Header("Progress Text Setup")]
    [SerializeField] private string preMessage;
    [SerializeField] private string centerMessage;
    [SerializeField] private string postMessage;
    [SerializeReference] private TextMeshProUGUI progressText;

    [Header("Progress Image Setup")]
    [SerializeReference] private Image progressBar;

    private void Start()
    {
        t = SavedValue;
        UpdateUI(t, range);
        GameManager.instance.playerExprence = this;
    }

    private float SavedValue
    {
        get
        {
            if (PlayerPrefs.HasKey(GetID))
                return PlayerPrefs.GetFloat(GetID);
            else
                PlayerPrefs.SetFloat(GetID, t);
            return t;
        }
        set
        {
            PlayerPrefs.SetFloat(GetID, value);
        }
    }

    private void UpdateUI(float t, Vector2 range)
    {
        if(progressBar)
            progressBar.fillAmount = t;

        if (progressText)
        { 
            var message = preMessage + Mathf.Lerp(range.x, range.y, t) +centerMessage + range.y + postMessage;
            progressText.text = message;
        }
    }
    public void AddExprence(float dt)
    {
        t += dt;
        t = Mathf.Clamp01(t);
        SavedValue = t;

        UpdateUI(t, range);
    }
}
