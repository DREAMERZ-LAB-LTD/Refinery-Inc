using IdleArcade.Core;
using System.Collections.Generic;
using UnityEngine;

public class StatusHandler : MonoBehaviour
{
    [SerializeField] private GameObject rootPanel;
    [SerializeField] private Transform contentHolder;
    [SerializeField] private Entity[] statusPrefabs;
    private List<Entity> activeStatus = new List<Entity>();


    private void Start()
    {
        UpgradeSystem.instance.OnAddStatus += AddUpgradeStatus;
        UpgradeSystem.instance.OnRemoveStatus += RemoveUpgradeStatus;
    }

    private void OnDestroy()
    {
        if (UpgradeSystem.instance == null) return;

        UpgradeSystem.instance.OnAddStatus -= AddUpgradeStatus;
        UpgradeSystem.instance.OnRemoveStatus -= RemoveUpgradeStatus;
    }

    public void Clear()
    {
        for (int i = 0; i < activeStatus.Count; i++)
            Destroy(activeStatus[i].gameObject);
        
        activeStatus.Clear();
        rootPanel.SetActive(false);
    }

    private Entity GetPrefab(string id)
    {
        for (int i = 0; i < statusPrefabs.Length; i++)
        {
            if (id == statusPrefabs[i].GetID)
                return statusPrefabs[i];
        }
#if UNITY_EDITOR
        Debug.Log("<color=cyan> There are no any prefab found with ID '" + id + "' Under the gameobject of" + name + "</color>");
#endif

        return null;
    }

    private Entity SpawnEntity(string prefabID)
    {
        var prefab = GetPrefab(prefabID);
        if (prefab == null) return null;

        var statusObj = Instantiate(prefab.gameObject, contentHolder);
        statusObj.SetActive(true);

        var newEntity = statusObj.GetComponent<Entity>();
        return newEntity;
    }

    private void AddUpgradeStatus(UpgradeableDataFields.Data data, string prefabID)
    {
        var entity = SpawnEntity(prefabID);

        var upgradeStatus = entity as AbilityStatus;
        if (upgradeStatus == null)
        { 
            Destroy(entity.gameObject);
            return;
        }


        upgradeStatus.Data = data;
        activeStatus.Add(entity);

        if (!rootPanel.activeInHierarchy)
            rootPanel.SetActive(true);
    }

    private void RemoveUpgradeStatus(UpgradeableDataFields.Data data, string prefabID)
    {
        AbilityStatus upgradeStatus = null;
        for (int i = 0; i < activeStatus.Count; i++)
        {
            upgradeStatus = activeStatus[i] as AbilityStatus;
            if (upgradeStatus == null) continue;

            if (upgradeStatus.Data == data)
            {
                activeStatus.RemoveAt(i);
                Destroy(upgradeStatus.gameObject);
                break;
            }
        }

        if (rootPanel.activeInHierarchy && activeStatus.Count == 0)
            rootPanel.SetActive(false);
    }
}
