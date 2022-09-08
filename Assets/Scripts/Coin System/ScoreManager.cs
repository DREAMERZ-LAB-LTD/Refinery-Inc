using UnityEngine;
using System.Collections;
using TMPro;

namespace General.Library
{
    public class ScoreManager : MonoBehaviour
    {
        #region SingleTon
        private static ScoreManager _instance = null;
        public static ScoreManager instance
        {
            get
            {
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }
        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
        #endregion SingleTon

        [System.Serializable]
        private class PriceList
        {
            public string id;
            public int price;
        }

        [Header("Text Message Setup")]
        [SerializeField] private string preMessage = string.Empty;
        [SerializeField] private string postMessage = string.Empty;
        [SerializeField] private TextMeshProUGUI coin_text;

        [Header("References Setup")]
        [SerializeField] private ScoreData scoreData;
        [SerializeField] private PriceList[] priceLists;

        private void OnEnable()
        {
            UpdateUIStatus(0, scoreData.Score);
            scoreData.OnScoreChanged += UpdateUIStatus;
        }
        private void OnDisable() => scoreData.OnScoreChanged -= UpdateUIStatus;
        
        private void UpdateUIStatus(int dt, int newScore)
        {
            if (coin_text == null) return;
            coin_text.text = preMessage + newScore + postMessage;
        }

        public int GetPrice(string id)
        {
            for (int i = 0; i < priceLists.Length; i++)
                if (priceLists[i].id == id)
                    return priceLists[i].price;
            return 0;
        }


        public void OnChangedAddListner(ScoreData.ScoreUpdate callback)
        {
            scoreData.OnScoreChanged += callback;
        }
        public void OnChangedRemoveListner(ScoreData.ScoreUpdate callback)
        {
            scoreData.OnScoreChanged -= callback;
        }

        /// <summary>
        /// Update score to Scriptable and UI (+) amount will increase score and (-) amount will decrease socre
        /// </summary>
        /// <param name="dt">Delta amount</param>
        public bool AddScore(int dt) => scoreData.AddScore(dt);
        

        public void AddScoreAsync(int amount)
        {
            var data = scoreData;
            if (data == null) return;

            StartCoroutine(AddscoreAscync(amount));
            IEnumerator AddscoreAscync(int amount)
            {
                var startTime = Time.time;
                var endTime = startTime + 2;
                var startScore = data.Score;
                var targetScore = startScore + amount;
                var t = 0.0f;
                data.SetScore(targetScore);

                while (Time.time <= endTime)
                {
                    t = Mathf.InverseLerp(startTime, endTime, Time.time);
                    amount = (int) Mathf.Lerp(startScore, targetScore, t);
                    UpdateUIStatus(0, amount);
                    yield return null;
                }
                UpdateUIStatus(0, targetScore);
            }
        }
    }
}
