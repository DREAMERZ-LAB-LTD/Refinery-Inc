using IdleArcade.Core;
using General.Library;
using UnityEngine;
using TMPro;

public class PlayerCoinContainer : TransactionContainer
{

    [Header("Coin saveing file")]
    [SerializeField] private ScoreData scoreData;

    [Header("Text Message Setup")]
    [SerializeField] private string preMessage = string.Empty;
    [SerializeField] private string postMessage = string.Empty;
    [SerializeField] private TextMeshProUGUI coin_text;

    protected void Start()
    {
        m_amount = scoreData.Score;
        scoreData.OnScoreChanged += UpdateUIStatus;
        OnChangedValue += OnChange;
        UpdateUIStatus(0, m_amount);

        GameManager.instance.coinContainer = this;
    }
    private void OnDestroy()
    {
        scoreData.OnScoreChanged -= UpdateUIStatus;
        OnChangedValue -= OnChange;
        GameManager.instance.coinContainer = null;
    }



    private void UpdateUIStatus(int dt, int newScore)
    {
        if (coin_text == null) return;
        coin_text.text = preMessage + newScore + postMessage;
    }

    private void OnChange(int delta, int currnet, int max, string containerID, TransactionContainer A, TransactionContainer B)
    {
        scoreData.AddScore(delta);
        if (name == "Conin Collector")
            Debug.Log("Delta " + delta);
    }
}
