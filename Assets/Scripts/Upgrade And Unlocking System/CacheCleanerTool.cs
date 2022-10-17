using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CacheCleanerTool : MonoBehaviour
{
    [MenuItem("Tools/Clean Cache")]
    private static void CleanCache()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Chache Cleaned");
    }
}
