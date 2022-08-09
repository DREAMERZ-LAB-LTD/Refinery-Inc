using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    [SerializeField] private UnlockingData data;
    GameObject unlockableObject;

    private void Awake()
    {
        if (data)
            if (data.IsUnlocked)
            {
                unlockableObject.SetActive(true);
                Destroy(this);
            }
    }

    public void Unlock()
    {
        if (data)
        { 
            if(data.Unlock())
                unlockableObject.SetActive(true);
        }
    }
}
