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

        public delegate void ChangeStatus(UpgradeableDataFields.Data data, string prefabID);
        public ChangeStatus OnAddStatus;
        public ChangeStatus OnRemoveStatus;
        [SerializeField] private string prefabID;
        [SerializeField] private UpgradeableDataFields upgradeableData;


        public UpgradeableDataFields.Data GetDataField(string id)
        {
            for (int i = 0; i < upgradeableData.fields.Length; i++)
                if (id == upgradeableData.fields[i].iD)
                    return upgradeableData.fields[i];

            return null;
        }

        public void Add(string[] upgradeableIDs)
        {
            for (int i = 0; i < upgradeableIDs.Length; i++)
            { 
                var data = GetDataField(upgradeableIDs[i]);
                if (data == null) continue;

                if(OnAddStatus!= null)
                    OnAddStatus.Invoke(data, prefabID);
            }
        }

        public void Remove(string[] upgradeableIDs)
        {
            for (int i = 0; i < upgradeableIDs.Length; i++)
            {
                var data = GetDataField(upgradeableIDs[i]);
                if (data == null) continue;

                if(OnRemoveStatus != null)
                    OnRemoveStatus.Invoke(data, prefabID);
            }
        }
    }
}