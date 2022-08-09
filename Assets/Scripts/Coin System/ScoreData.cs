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
        public int Score => score;
        public bool AddScore(int dt)
        {
            if (dt < 0)
                if (dt < score)
                    return false;
            score += dt;
            score = (int)Mathf.Clamp(score, 0, Mathf.Infinity);
            if (OnScoreChanged != null)
                OnScoreChanged.Invoke(dt, score);

            return true;
        }

        public void SetScore(int newScore)
        {
            score = newScore;
            if (OnScoreChanged != null)
                OnScoreChanged.Invoke(newScore, score);
        }
    }
}