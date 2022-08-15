using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public class UpgradeSystem : MonoBehaviour
    {
        #region SingleTon
        private static UpgradeSystem _instance = null;
        public static UpgradeSystem instance
        {
            get
            {
                return _instance;
            }
        }
        protected virtual void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }
        protected virtual void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
        #endregion SingleTon

        [SerializeField] private UpgradeableDataFields upgradeableData;
        public List<UpgradeableDataFields.Data> activeFields = new List<UpgradeableDataFields.Data>();

        [SerializeField] private Transform contentHolder;
        [SerializeField] private UpgradeStatus statusPrefab;
        private List<UpgradeStatus> activeStatus = new List<UpgradeStatus>();
        public UpgradeableDataFields.Data GetDataField(string id)
        {
            for (int i = 0; i < upgradeableData.fields.Length; i++)
                if (id == upgradeableData.fields[i].ID)
                    return upgradeableData.fields[i];

            return null;
        }

        public void Add(string[] upgradeableIDs)
        {
            for (int i = 0; i < upgradeableIDs.Length; i++)
            { 
                var data = GetDataField(upgradeableIDs[i]);
                if (data == null) continue;
                if (activeFields.Contains(data)) continue;

                activeFields.Add(data);
                AddStatus(data);
            }
        }

        public void Remove(string[] upgradeableIDs)
        {
            for (int i = 0; i < upgradeableIDs.Length; i++)
            {
                var data = GetDataField(upgradeableIDs[i]);
                if (data == null) continue;
                if (!activeFields.Contains(data)) continue;

                activeFields.Remove(data);
                RemoveStatus(data);
            }
        }

        private void AddStatus(UpgradeableDataFields.Data data)
        {
            var statusObj = Instantiate(statusPrefab.gameObject, contentHolder);
            statusObj.SetActive(true);
            var staus = statusObj.GetComponent<UpgradeStatus>();
            if (staus == null)
                Destroy(statusObj);
            else
            {
                staus.Data = data;
                activeStatus.Add(staus);
            
            }
        }

        private void RemoveStatus(UpgradeableDataFields.Data data)
        {

            for (int i = 0; i < activeStatus.Count; i++)
            {
                if (activeStatus[i].Data == data)
                {
                    var statusObj = activeStatus[i].gameObject;
                    activeStatus.RemoveAt(i);
                    Destroy(statusObj);
                    break;
                }
            }

            
        }

    }
}