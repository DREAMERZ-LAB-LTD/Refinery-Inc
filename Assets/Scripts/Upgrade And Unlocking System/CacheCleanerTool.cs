using UnityEditor;
using UnityEngine;

public class CacheCleanerTool : MonoBehaviour
{
#if UNITY_EDITOR

    [MenuItem("Tools/Clean Cache")]
    private static void CleanCache()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Chache Cleaned");
    }
#endif
}
