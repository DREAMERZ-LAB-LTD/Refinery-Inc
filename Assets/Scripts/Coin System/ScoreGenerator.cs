using UnityEngine;

namespace General.Library
{
    public class ScoreGenerator : MonoBehaviour
    {
        [SerializeField] string targetID;
        [SerializeField] private int amount = 1;

        public virtual void GenerateScore()
        {
            if (enabled)
                ScoreManager.instance.AddScore(amount, targetID);
        }
    }
}