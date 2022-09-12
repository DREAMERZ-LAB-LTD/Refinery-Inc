using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region SingleTon
    private static GameManager _instance = null;
    public static GameManager instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        if (_instance != this)
            Destroy(gameObject);
    }
    #endregion

    [HideInInspector] public PlayerExprence playerExprence;
}
