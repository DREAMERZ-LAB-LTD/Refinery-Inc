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
        { 
            _instance = this;
            OnInit();
        }
        if (_instance != this)
            Destroy(gameObject);
    }
    #endregion


    [HideInInspector] public PlayerExprence playerExprence;
    [HideInInspector]public ObjectPullingSystem pullingSystem;
    private Joystick joystick = null;
    
    public Joystick Joystic
    {
        get
        {
            if (joystick == null)
                joystick = FindObjectOfType<Joystick>();

            return joystick;
        }
    }
   
    
    private void OnInit()
    {
        if (joystick == null)
            joystick = FindObjectOfType<Joystick>();
    }
}
