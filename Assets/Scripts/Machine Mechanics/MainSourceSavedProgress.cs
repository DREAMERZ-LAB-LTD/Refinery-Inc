using UnityEngine;
using IdleArcade.Core;
public class MainSourceSavedProgress : ContainerSavedStatus
{
    [SerializeField] private string cacheID = "Progress";
    [SerializeField] private int initialAmount = 20;
    protected override int SavedAmount 
    {
        set => base.SavedAmount = value;
        get
        {
            if (PlayerPrefs.HasKey(cacheID))
                return PlayerPrefs.GetInt(cacheID);
            else
            { 
                PlayerPrefs.SetInt(cacheID, initialAmount);
                return initialAmount;
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }
}
