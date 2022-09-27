using UnityEngine;

namespace General.Library
{
    [CreateAssetMenu(fileName = "New Score Data", menuName = "Dlab/Score/Data")]
    public class ScoreData : ScriptableObject
    {
        public delegate void ScoreUpdate(int dt, int newScore);
        public ScoreUpdate OnScoreChanged;

        [SerializeField] private string iD;
        [SerializeField] private int score = 0;
        public string ID => iD;
        public int Score
        {
            get 
            {
                if (PlayerPrefs.HasKey(iD))
                    return PlayerPrefs.GetInt(iD);

                return score;
            }
            set
            {
                PlayerPrefs.SetInt(iD, value);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Score = score;
        }
#endif

        public bool AddScore(int dt)
        {
            int score = Score;
            if (dt < 0)
                if (score + dt < 0)
                    return false;
            score += dt;
            if (score < 0)
                score = 0;
            Score = score;

            if (OnScoreChanged != null)
                OnScoreChanged.Invoke(dt, score);

            return true;
        }

        public void SetScore(int newScore)
        {
            Score = newScore;
            if (OnScoreChanged != null)
                OnScoreChanged.Invoke(newScore, newScore);
        }
    }
}