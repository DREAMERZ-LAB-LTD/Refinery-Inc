using UnityEngine;

namespace IdleArcade.Core
{
    [CreateAssetMenu(fileName = "New Upgradeable Data", menuName = "Idle Arcade/Upgrade/ Upgradeable Data")]
    public class UpgradeableDataFields : ScriptableObject
    {
        public delegate void ChangedFieldValue(float t);
        public delegate void ChangedUnlockValue(bool isUnlocked);
        [System.Serializable] public class Data
        {
            public ChangedFieldValue OnChanged;
            public ChangedUnlockValue OnUnlocking;

            [Header("Visual Element")]
            [SerializeField] public string iD;
            [SerializeField] public string name;
            [SerializeField] public Sprite icon;

            [Header("Upgrade & Unlocking Status")]
            [SerializeField] private bool m_isUnlocked = false;
            [SerializeField, Range(0, 1)]  private float t = 0.2f;
            [SerializeField, Range(0, 1)]  public float dt = 0.1f;

            [Header("Pricing Setup")]
            [SerializeField] public string coinID;
            [SerializeField] public int unlockPrice = 10;
            [SerializeField] public int upgradePrice = 10;


            public bool isUpgraded => t == 1;

            public bool isUnlocked
            {
                get { return m_isUnlocked; }
                set
                {
                    m_isUnlocked = value;
                    OnUnlocking.Invoke(value);
                }
            }

            public float T
            {
                get
                {
                    return t;
                }
                set
                {
                    t = Mathf.Clamp01(value);
                
                    if(OnChanged!= null)
                        OnChanged.Invoke(t);
                }
            }
        }


        [Header("Upgradeable Data Fields")]
        [SerializeField] public Data[] fields;
    }
}