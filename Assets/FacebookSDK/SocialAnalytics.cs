using Facebook.Unity;
using UnityEngine;

public class SocialAnalytics : MonoBehaviour
{
    private void Awake()
    {
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
            },
            isGameShown =>
            {
                if (!isGameShown)
                {
#if UNITY_EDITOR
                    Debug.Log("IS not Game Shown");
#endif
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("IS Game Shown");
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
