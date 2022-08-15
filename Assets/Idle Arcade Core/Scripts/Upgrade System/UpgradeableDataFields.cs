using UnityEngine;

namespace IdleArcade.Core
{
    [CreateAssetMenu(fileName = "New Upgradeable Data", menuName = "Idle Arcade/Upgrade/ Upgradeable Data")]
    public class UpgradeableDataFields : ScriptableObject
    {
        public delegate void ChangedFieldValue(float t);
        [System.Serializable] public class Data
        {
            public ChangedFieldValue OnFieldChanged;

            [SerializeField] private string name;
            [SerializeField] private string iD;
            [SerializeField, Range(0, 1)] private float t = 0;

            public string Name => name;
            public string ID => iD;
            public bool isUpgraded => t == 1;

            public float T
            {
                get
                {
                    return t;
                }
                set
                {
                    t = value;
                    Mathf.Clamp01(t);
                
                    if(OnFieldChanged!= null)
                        OnFieldChanged.Invoke(t);
                }
            }
        }


        [Header("Upgradeable Data Fields")]
        [SerializeField] public Data[] fields;
    }
}