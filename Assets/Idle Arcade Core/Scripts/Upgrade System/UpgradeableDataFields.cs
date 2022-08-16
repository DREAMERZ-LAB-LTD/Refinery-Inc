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

            [Header("Visual Element")]
            [SerializeField] public string iD;
            [SerializeField] public string name;
            [SerializeField] public Sprite icon;

            [Header("Pricing Setup")]
            [SerializeField] public string coinID;
            [SerializeField] public int unlockPrice = 10;
            [SerializeField] public int upgradePrice = 10;

            [Header("Upgrade & Unlocking Status")]
            public bool isUnlocked = false;
            [SerializeField, Range(0, 1)]  private float t = 0.2f;
            [SerializeField, Range(0, 1)]  public float dt = 0.1f;

            public bool isUpgraded => t == 1;

            public float T
            {
                get
                {
                    return t;
                }
                set
                {
                    t = Mathf.Clamp01(value);
                
                    if(OnFieldChanged!= null)
                        OnFieldChanged.Invoke(t);
                }
            }
        }


        [Header("Upgradeable Data Fields")]
        [SerializeField] public Data[] fields;
    }
}