using UnityEngine;

public class PauseResume : MonoBehaviour
{
    public void OnClickPause()
    {
        Time.timeScale = 0;
    }

    public void OnClickResume()
    { 
        Time.timeScale = 1;
    }
    public void OnClickExit()
    {
        Application.Quit();
    }
}
