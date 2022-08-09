using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IdleArcade.Core
{
    public class UpgradeSystem : MonoBehaviour
    {
        [SerializeField] private UpgradeableField[] dataFields;

        public UpgradeableField GetDataField(string id)
        {
            for (int i = 0; i < dataFields.Length; i++)
                if (id == dataFields[i].ID)
                    return dataFields[i];

            return null;
        }
        public void OnChangedAddListner(string id, UpgradeableField.FieldChanged callback)
        {
            var data = GetDataField(id);
            data.OnFieldChanged += callback;
        }
        public void OnChangedRemoveListner(string id, UpgradeableField.FieldChanged callback)
        {
            var data = GetDataField(id);
            data.OnFieldChanged -= callback;
        }

        public bool Upgrade(float dt, string id)
        {
            var data = GetDataField(id);

            if (data.ID != id) return false;
            if (data.isUpgraded) return false;

            data.T += Mathf.Abs(dt); 
            return true;
        }
    }
}