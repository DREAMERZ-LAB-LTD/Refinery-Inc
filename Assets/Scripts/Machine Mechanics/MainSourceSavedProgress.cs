using UnityEngine;
using IdleArcade.Core;
public class MainSourceSavedProgress : ContainerSavedStatus
{
    [SerializeField] private int initialAmount = 20;
    protected override int SavedAmount 
    {
        set => base.SavedAmount = value;
        get
        {
            if (PlayerPrefs.HasKey("Progress"))
                return PlayerPrefs.GetInt("Progress");
            else
            { 
                PlayerPrefs.SetInt("Progress", initialAmount);
                return initialAmount;
            }
        }
    }

    protected override void Start()
    {
        base.Start();
    }
}
