using TMPro;
using UnityEngine;

namespace General.Library
{
    public class ScoreFieldStatus : MonoBehaviour
    {
        [SerializeField] private ScoreData scoreData;
        [Header("Text Message Setup")]
        [SerializeField] private TextMeshProUGUI coin_text;
        [SerializeField] private string preMessage = string.Empty;
        [SerializeField] private string postMessage = string.Empty;

        private void UpdateScoreUI(int st, int newScore)
        {
            if (coin_text == null) return;
            coin_text.text = preMessage + newScore + postMessage;
        }

        private void OnEnable()
        {
            if (scoreData != null)
            {
                UpdateScoreUI(0, scoreData.Score);
                scoreData.OnScoreChanged += UpdateScoreUI;
            }
        }
        private void OnDisable()
        {
            if (scoreData != null)
                scoreData.OnScoreChanged -= UpdateScoreUI;

        }
    }
}
