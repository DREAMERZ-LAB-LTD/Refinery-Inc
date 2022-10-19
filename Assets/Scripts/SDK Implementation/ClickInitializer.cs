using UnityEngine;
using Tabtale.TTPlugins;

public class ClickInitializer : MonoBehaviour
{
    private void Awake()
    {
        // Initialize CLIK Plugin
        TTPCore.Setup();
        // Your code here
    }
}
