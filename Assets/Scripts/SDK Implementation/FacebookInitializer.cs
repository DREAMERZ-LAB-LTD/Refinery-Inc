using Facebook.Unity;
using UnityEngine;

public class FacebookInitializer : MonoBehaviour
{
    public static FacebookInitializer instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        if (instance != this) Destroy(gameObject);

        InitilizeFaceook();
    }
    

    private void InitilizeFaceook()
    {

        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                {
                    FB.ActivateApp();
#if UNITY_EDITOR
                    Debug.Log("IsInitialized");
#endif
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("Couldn't initialize");
#endif
                }
            });
        }
        else
        {
            FB.ActivateApp();
        }
    }
}
