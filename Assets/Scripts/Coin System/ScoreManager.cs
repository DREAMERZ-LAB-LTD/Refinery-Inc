using UnityEngine;
using System.Collections;

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

        [SerializeField] private ScoreData[] scoreDatas;
        protected ScoreData GetScoreData(string id)
        {
            for (int i = 0; i < scoreDatas.Length; i++)
                if (id == scoreDatas[i].ID)
                    return scoreDatas[i];

            return null;
        }

        public void OnChangedAddListner(string id, ScoreData.ScoreUpdate callback)
        {
            var data = GetScoreData(id);
            data.OnScoreChanged += callback;
        }
        public void OnChangedRemoveListner(string id, ScoreData.ScoreUpdate callback)
        {
            var data = GetScoreData(id);
            data.OnScoreChanged -= callback;
        }

        /// <summary>
        /// Update score to Scriptable and UI (+) amount will increase score and (-) amount will decrease socre
        /// </summary>
        /// <param name="dt">Delta amount</param>
        public bool AddScore(int dt, string id)
        {
            var scoreData = GetScoreData(id);
            return scoreData.AddScore(dt);
        }

        public void AddScoreAsync(int amount, string id, float targetFreamCount = 30f)
        {
            var scoreData = GetScoreData(id);
            if (scoreData == null) return;

            StartCoroutine(AddscoreAscync(amount));
            IEnumerator AddscoreAscync(int amount)
            {
                float delta = 1 / targetFreamCount;
                float numberSegment = amount * delta;
                int totalSum = scoreData.Score + amount;

                while (targetFreamCount > 0)
                {
                    targetFreamCount--;
                    scoreData.AddScore(Mathf.CeilToInt(numberSegment));
                    yield return null;
                }
                scoreData.SetScore(totalSum);
            }
        }
    }
}
