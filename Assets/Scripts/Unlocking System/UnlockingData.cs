using UnityEngine;
using General.Library;

[CreateAssetMenu(fileName = "New Unlocking Data", menuName = "Idle Arcade/Unlocking/Data")]
public class UnlockingData : ScriptableObject
{
    [SerializeField] private string coinID;
    [SerializeField] bool unlocked = false;
    [SerializeField] private int price;

    public bool IsUnlocked => unlocked;

    public bool Unlock()
    {
        if (unlocked) return false;

        if (ScoreManager.instance.AddScore(price, coinID))
        {
            unlocked = true;
            return true;
        }
        return false;
    }
}
